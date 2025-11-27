using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class FinalContinuationBuilderExtensions
    {
        public static FinalContinuationBuilder WithNewProjection(this FinalContinuationBuilder builder, Projection newProjection) => builder
            .WithNewProjection(static (_, newProjection) => newProjection, newProjection);

        public static FinalContinuationBuilder WithNewProjection(this FinalContinuationBuilder builder, Func<Projection, Projection> projectionTransformation) => builder
            .WithNewProjection(
                static (projection, projectionTransformation) => projectionTransformation(projection),
                projectionTransformation);

        public static FinalContinuationBuilder Where(this FinalContinuationBuilder builder, Traversal traversal) => builder
            .AddSteps(traversal.Count > 0 && traversal.Steps.All(static x => x is IFilterStep)
                ? traversal.Steps
                : [new FilterStep.ByTraversalStep(traversal)]);

        public static FinalContinuationBuilder None(this FinalContinuationBuilder builder) => builder
            .WithSteps(static traversal => traversal.IsIdentity()
                ? NoneStep.Instance
                : traversal.Push(NoneStep.Instance));

        public static FinalContinuationBuilder OfType<TElement, TNewElement>(this FinalContinuationBuilder builder, IGraphElementModel model, bool force)
        {
            if (typeof(TNewElement) != typeof(object) && (force || !typeof(TNewElement).IsAssignableFrom(typeof(TElement))))
            {
                var labels = model.TryGetFilterLabels(typeof(TNewElement), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity)) ?? ImmutableArray.Create(typeof(TNewElement).Name);

                if (labels.Length > 0)
                    builder = builder.AddStep(new HasLabelStep(labels));
            }

            return builder;
        }

        public static FinalContinuationBuilder And(this FinalContinuationBuilder builder, Span<Traversal> traversals)
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

        public static FinalContinuationBuilder Or(this FinalContinuationBuilder builder, Span<Traversal> traversals)
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
    }
}
