using System.Collections.Generic;
using System.Linq;

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
            if (_traversal.Steps.Count == 0 || !(_traversal.Steps[_traversal.Steps.Count - 1] is HasStep hasStep) || !hasStep.Value.Exists(hasStepValue => hasStepValue == P.False))
                yield return MethodStep.Create("not", _traversal);
        }
    }
}
