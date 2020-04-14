using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public abstract class LogicalStep<TStep> : Step
        where TStep : LogicalStep<TStep>
    {
        protected LogicalStep(string name, Traversal[] traversals)
        {
            Name = name;
            Traversals = traversals
                .SelectMany(FlattenLogicalTraversals)
                .ToArray();
        }

        private static IEnumerable<Traversal> FlattenLogicalTraversals(Traversal traversal)
        {
            if (traversal.Steps.SingleOrDefault() is TStep otherStep)
            {
                foreach (var subTraversal in otherStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenLogicalTraversals(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return traversal;
        }

        public string Name { get; }
        public Traversal[] Traversals { get; }
    }
}
