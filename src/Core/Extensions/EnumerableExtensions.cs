using System.Collections;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public static class EnumerableExtensions
    {
        //TODO: Remove on breaking change.
        public static Traversal ToTraversal(this IEnumerable<Step> source)
        {
            if (source is ICollection sourceCollection)
            {
                var newSteps = new Step[sourceCollection.Count];

                sourceCollection.CopyTo(newSteps, 0);

                return new(newSteps, Projection.Empty);
            }

            var ret = Traversal.Empty;

            foreach (var step in source)
            {
                ret = ret
                    .Push(step);
            }

            return ret;
        }
    }
}
