using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public abstract class LogicalStep : NonTerminalStep
    {
        protected LogicalStep(string name, IGremlinQuery[] traversals)
        {
            Name = name;
            Traversals = traversals;
        }

        public string Name { get; }
        public IGremlinQuery[] Traversals { get; }
    }

    public abstract class LogicalStep<TStep> : LogicalStep where TStep : LogicalStep
    {
        protected LogicalStep(string name, IGremlinQuery[] traversals) : base(name, traversals)
        {
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new ResolvedMethodStep(
                Name,
                Traversals
                    .SelectMany(traversal => FlattenTraversals(traversal))
                    .Select(traversal => traversal.Resolve(model))
                    .ToArray<object>());
        }

        private IEnumerable<IGremlinQuery> FlattenTraversals(IGremlinQuery query)
        {
            if (query.Steps.Count == 2 && query.Steps[1] is TStep andStep)
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
    }
}
