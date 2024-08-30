using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class FinalContinuationBuilderExtensions
    {
        public static FinalContinuationBuilder<TOuterQuery, TOuterQuery> AddSteps<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, IEnumerable<Step> steps)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        {
            foreach (var step in steps)
            {
                builder = builder.AddStep(step);
            }

            return builder;
        }

        public static FinalContinuationBuilder<TOuterQuery, TOuterQuery> AddSteps<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Traversal traversal)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        {
            for (var i = 0; i < traversal.Count; i++)
            {
                builder = builder.AddStep(traversal[i]);
            }

            return builder;
        }

        public static FinalContinuationBuilder<TOuterQuery, TOuterQuery> WithNewProjection<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Projection newProjection)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder.WithNewProjection(static (_, newProjection) => newProjection, newProjection);

        public static FinalContinuationBuilder<TOuterQuery, TOuterQuery> WithNewProjection<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Func<Projection, Projection> projectionTransformation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase =>
            builder.WithNewProjection(
                static (projection, projectionTransformation) => projectionTransformation(projection),
                projectionTransformation);

        public static FinalContinuationBuilder<TOuterQuery, TOuterQuery> Where<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Traversal traversal)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder
                .AddSteps(traversal.Count > 0 && traversal.Steps.All(static x => x is IFilterStep)
                    ? traversal
                    : new FilterStep.ByTraversalStep(traversal));

        public static FinalContinuationBuilder<TOuterQuery, TOuterQuery> None<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder.WithSteps(
                static (traversal, _) => traversal.IsIdentity()
                    ? NoneStep.Instance
                    : traversal.Push(NoneStep.Instance),
                0);
    }
}
