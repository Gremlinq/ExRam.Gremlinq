using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class TraversalArgumentStep : NonTerminalStep
    {
        private readonly string _name;
        private readonly IGremlinQuery[] _traversals;

        public TraversalArgumentStep(string name, params IGremlinQuery[] traversals)
        {
            _name = name;
            _traversals = traversals;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return MethodStep.Create(_name, _traversals.Select(x => x.Resolve(model)).ToArray<object>());
        }
    }
}
