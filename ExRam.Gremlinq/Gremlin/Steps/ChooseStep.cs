using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class ChooseStep : NonTerminalStep
    {
        private readonly IGremlinQuery _ifTraversal;
        private readonly IGremlinQuery _thenTraversal;
        private readonly IGremlinQuery _elseTraversal;

        public ChooseStep(IGremlinQuery ifTraversal, IGremlinQuery thenTraversal, IGremlinQuery elseTraversal)
        {
            _ifTraversal = ifTraversal;
            _thenTraversal = thenTraversal;
            _elseTraversal = elseTraversal;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return MethodStep.Create(
                "choose",
                _ifTraversal.Resolve(model),
                _thenTraversal.Resolve(model),
                _elseTraversal.Resolve(model));
        }
    }
}