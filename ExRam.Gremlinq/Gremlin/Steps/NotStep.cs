using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class NotStep : NonTerminalStep
    {
        private readonly IGremlinQuery _traversal;

        public NotStep(IGremlinQuery traversal)
        {
            _traversal = traversal;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            if (_traversal.Steps.Count == 0 || !(_traversal.Steps[_traversal.Steps.Count - 1] is HasStep hasStep) || hasStep.Value != P.False)
                yield return MethodStep.Create("not", _traversal.Resolve(model));
        }
    }
}
