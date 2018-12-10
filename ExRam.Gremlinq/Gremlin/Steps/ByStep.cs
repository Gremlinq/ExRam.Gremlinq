using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class ByStep : NonTerminalStep
    {
        private readonly Order _order;
        private readonly IGremlinQuery _traversal;

        public ByStep(IGremlinQuery traversal, Order order)
        {
            _order = order;
            _traversal = traversal;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new ResolvedMethodStep("by", _traversal.Resolve(model), _order);
        }
    }
}
