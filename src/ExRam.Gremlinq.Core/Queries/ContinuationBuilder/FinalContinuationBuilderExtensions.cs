using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class FinalContinuationBuilderExtensions
    {
        public static FinalContinuationBuilder<TOuterQuery> AddSteps<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, IEnumerable<Step> steps)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        {
            foreach (var step in steps)
            {
                builder = builder.AddStep(step);
            }

            return builder;
        }

        public static FinalContinuationBuilder<TOuterQuery> AddSteps<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Traversal traversal)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        {
            for (var i = 0; i < traversal.Count; i++)
            {
                builder = builder.AddStep(traversal[i]);
            }

            return builder;
        }

        public static FinalContinuationBuilder<TOuterQuery> WithNewProjection<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Projection newProjection)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase => builder.WithNewProjection(static (_, newProjection) => newProjection, newProjection);

        public static FinalContinuationBuilder<TOuterQuery> WithNewProjection<TOuterQuery>(this FinalContinuationBuilder<TOuterQuery> builder, Func<Projection, Projection> projectionTransformation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase =>
            builder.WithNewProjection(
                static (projection, projectionTransformation) => projectionTransformation(projection),
                projectionTransformation);
    }
}
