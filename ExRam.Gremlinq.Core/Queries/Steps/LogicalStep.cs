using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public abstract class LogicalStep<TStep> : Step
        where TStep : LogicalStep<TStep>
    {
        protected LogicalStep(string name, IGremlinQueryBase[] traversals)
        {
            Name = name;
            Traversals = traversals
                .SelectMany(FlattenLogicalTraversals)
                .ToArray();
        }

        private static IEnumerable<IGremlinQueryBase> FlattenLogicalTraversals(IGremlinQueryBase query)
        {
            var steps = query.AsAdmin().Steps;

            if (!steps.IsEmpty && steps.Pop().IsEmpty && steps.Peek() is TStep otherStep)
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
                yield return query;
        }

        public string Name { get; }
        public IGremlinQueryBase[] Traversals { get; }
    }
}
