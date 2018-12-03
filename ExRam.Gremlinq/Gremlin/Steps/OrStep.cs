using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class OrStep : NonTerminalStep
    {
        public OrStep(IEnumerable<IGremlinQuery> traversals)
        {
            Traversals = traversals;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new MethodStep("or", Traversals
                .SelectMany(FlattenTraversals)
                .ToArray<object>());
        }

        private static IEnumerable<IGremlinQuery> FlattenTraversals(IGremlinQuery query)
        {
            if (query.Steps.Count == 2 && query.Steps[1] is OrStep orStep)
            {
                foreach (var subTraversal in orStep.Traversals)
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

        public IEnumerable<IGremlinQuery> Traversals { get; }
    }
}
