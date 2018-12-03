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
                .SelectMany(
                    query2 => query2.Steps.Count == 2 && query2.Steps[1] is AndStep andStep
                        ? andStep.Traversals
                        : new object[] { query2 })
                .ToArray());
        }

        public IGremlinQuery[] Traversals { get; }
    }
}