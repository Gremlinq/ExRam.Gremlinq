using System.Collections;
using System.Collections.Immutable;

using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class FinalContinuationBuilderExtensions
    {
        extension(FinalContinuationBuilder builder)
        {
            public FinalContinuationBuilder WithNewProjection(Projection newProjection) => builder
                .WithNewProjection(static (_, newProjection) => newProjection, newProjection);

            public FinalContinuationBuilder WithNewProjection(Func<Projection, Projection> projectionTransformation) => builder
                .WithNewProjection(
                    static (projection, projectionTransformation) => projectionTransformation(projection),
                    projectionTransformation);

            public FinalContinuationBuilder Where(Traversal traversal) => builder
                .AddSteps(traversal.Count > 0 && traversal.Steps.All(static x => x is IFilterStep)
                    ? traversal.Steps
                    : [new FilterStep.ByTraversalStep(traversal)]);

            public FinalContinuationBuilder None() => builder
                .WithSteps(static traversal => traversal.IsIdentity()
                    ? NoneStep.Instance
                    : traversal.Push(NoneStep.Instance));

            public FinalContinuationBuilder OfType(Type[] edgeTypes, IGraphElementModel model)
            {
                if (edgeTypes.Length > 0)
                {
                    var labels = model
                        .GetFilterLabels(edgeTypes, builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity));

                    if (labels.Length > 0)
                        builder = builder.AddStep(new HasLabelStep(labels));
                }

                return builder;
            }

            public FinalContinuationBuilder And(Span<Traversal> traversals)
            {
                if (traversals.Length == 0)
                    throw new ArgumentException("Expected at least 1 sub-query.");

                var count = 0;
                var containsNoneStep = false;
                var containsWriteStep = false;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.IsNone())
                        containsNoneStep = true;

                    if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                        containsWriteStep = true;
                    else if (traversal.IsIdentity())
                        continue;

                    traversals[count++] = traversal;
                }

                if (containsNoneStep && !containsWriteStep)
                    builder = builder.None();
                else
                {
                    var fusedTraversals = traversals[..count]
                        .Fuse(static (p1, p2) => p1.And(p2));

                    if (fusedTraversals is [var single])
                        builder = builder.Where(single);
                    else
                    {
                        if (fusedTraversals.All(static traversal => traversal.Steps.All(static x => x is IFilterStep)))
                        {
                            for (var i = 0; i < fusedTraversals.Length; i++)
                            {
                                builder = builder
                                    .AddSteps(fusedTraversals[i].Steps);
                            }
                        }
                        else
                        {
                            builder = builder
                                .AddStep(new AndStep(LogicalStep<AndStep>.FlattenLogicalTraversals(fusedTraversals)));
                        }
                    }
                }

                return builder;
            }

            public FinalContinuationBuilder Or(Span<Traversal> traversals)
            {
                if (traversals.Length == 0)
                    throw new ArgumentException("Expected at least 1 sub-query.");

                var count = 0;
                var containsWriteStep = false;
                var containsIdentityStep = false;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.IsIdentity())
                        containsIdentityStep = true;
                    else if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                        containsWriteStep = true;
                    else if (traversal.IsNone())
                        continue;

                    traversals[count++] = traversal;
                }

                if (!containsIdentityStep || containsWriteStep)
                {
                    var fusedTraversals = traversals[..count]
                        .Fuse(static (p1, p2) => p1.Or(p2));

                    builder = fusedTraversals switch
                    {
                        [] => builder
                            .None(),
                        [var singleTraversal] => builder
                            .Where(singleTraversal),
                        _ => builder
                            .AddStep(new OrStep(LogicalStep<OrStep>.FlattenLogicalTraversals(fusedTraversals)))
                    };
                }

                return builder;
            }

            public FinalContinuationBuilder AddPropertySteps(Key key, object value, bool allowExplicitCardinality, IGremlinQueryEnvironment environment)
            {
                if (value is not Traversal && value is IEnumerable enumerable && !environment.SupportsType(value.GetType()))
                {
                    if (!allowExplicitCardinality)
                        throw new NotSupportedException($"A value of type {value.GetType()} is not supported for property '{key}'.");

                    foreach (var item in enumerable)
                    {
                        builder = builder
                            .AddPropertyStep(key, item, Cardinality.List, environment);
                    }
                }
                else
                {
                    builder = builder
                        .AddPropertyStep(key, value, allowExplicitCardinality ? Cardinality.Single : null, environment);
                }

                return builder;
            }

            public FinalContinuationBuilder DateAdd(DT dateToken, int value) => value != 0
                ? builder.AddStep(new DateAddStep(dateToken, value))
                : builder;

            private FinalContinuationBuilder AddPropertyStep(Key key, object value, Cardinality? cardinality, IGremlinQueryEnvironment environment)
            {
                var actualValue = value;
                var metaProperties = ImmutableArray<KeyValuePair<string, object>>.Empty;

                if (actualValue is Property property)
                {
                    if (property is IVertexProperty vertexProperty)
                    {
                        metaProperties = vertexProperty
                            .GetProperties(environment)
                            .Select(static kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value))
                            .ToImmutableArray();
                    }

                    actualValue = property.GetValue();
                }

                if (actualValue != null)
                    builder = builder.AddStep(new PropertyStep.ByKeyStep(key, actualValue, metaProperties, cardinality));

                return builder;
            }
        }
    }
}
