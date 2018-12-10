using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class TraversalArgumentStep : NonTerminalStep
    {
        private readonly string _name;
        private readonly IGremlinQuery _traversal;

        public TraversalArgumentStep(string name, IGremlinQuery traversal)
        {
            _name = name;
            _traversal = traversal;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new ResolvedMethodStep(_name, new[] { _traversal.Resolve(model) });
        }
    }
}