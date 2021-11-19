using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class LogicalStep<TStep> : Step
        where TStep : LogicalStep<TStep>
    {
        protected LogicalStep(string name, IEnumerable<Traversal> traversals) : this(name, traversals.ToArray())
        {
            
        }

        private LogicalStep(string name, Traversal[] traversals) : base(traversals.GetSideEffectSemanticsChange())
        {
            Name = name;
            Traversals = traversals
                .SelectMany(FlattenLogicalTraversals)
                .ToImmutableArray();
        }

        private static IEnumerable<Traversal> FlattenLogicalTraversals(Traversal traversal)
        {
            if (traversal.Count == 1 && traversal[0] is TStep otherStep)
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
        public ImmutableArray<Traversal> Traversals { get; }
    }
}
