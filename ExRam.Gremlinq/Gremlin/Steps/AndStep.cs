using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class AndStep : NonTerminalStep
    {
        public AndStep(IGremlinQuery[] traversals)
        {
            Traversals = traversals;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new MethodStep("and", Traversals
                .SelectMany(FlattenTraversals)
                .ToArray<object>());
        }

        private static IEnumerable<IGremlinQuery> FlattenTraversals(IGremlinQuery query)
        {
            if (query.Steps.Count == 2 && query.Steps[1] is AndStep andStep)
            {
                foreach (var subTraversal in andStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenTraversals(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return query;
        }
    
        public IGremlinQuery[] Traversals { get; }
    }
}
