using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class FinalContinuationBuilderExtensions
    {
        public static FinalContinuationBuilder<TOuterQuery> WithNewProjection<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Projection newProjection)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder.WithNewProjection(static (_, newProjection) => newProjection, newProjection);

        public static FinalContinuationBuilder<TOuterQuery> WithNewProjection<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Func<Projection, Projection> projectionTransformation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase =>
            builder.WithNewProjection(
                static (projection, projectionTransformation) => projectionTransformation(projection),
                projectionTransformation);

        public static FinalContinuationBuilder<TOuterQuery> Where<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Traversal traversal)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder
                .AddSteps(traversal.Count > 0 && traversal.Steps.All(static x => x is IFilterStep)
                    ? traversal.Steps
                    : [new FilterStep.ByTraversalStep(traversal)]);

        public static FinalContinuationBuilder<TOuterQuery> None<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder.WithSteps(
                static traversal => traversal.IsIdentity()
                    ? NoneStep.Instance
                    : traversal.Push(NoneStep.Instance));

        public static FinalContinuationBuilder<TOuterQuery> OfType<TOuterQuery, TElement, TNewElement>(this FinalContinuationBuilder<TOuterQuery> builder, IGraphElementModel model, bool force)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        {
            if (typeof(TNewElement) != typeof(object) && (force || !typeof(TNewElement).IsAssignableFrom(typeof(TElement))))
            {
                var labels = model.TryGetFilterLabels(typeof(TNewElement), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity)) ?? ImmutableArray.Create(typeof(TNewElement).Name);

                if (labels.Length > 0)
                    builder = builder.AddStep(new HasLabelStep(labels));
            }

            return builder;
        }

        public static TOuterQuery Or<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Memory<Traversal> traversalsMemory)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        {
            var traversals = traversalsMemory.Span;

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

            return builder
                .Build();
        }
    }
}
