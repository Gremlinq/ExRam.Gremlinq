using System.Collections.Generic;
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
    }
}
