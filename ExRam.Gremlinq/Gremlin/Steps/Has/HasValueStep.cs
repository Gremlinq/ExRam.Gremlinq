using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class HasValueStep : NonTerminalStep
    {
        private readonly object _argument;

        public HasValueStep(object argument)
        {
            _argument = argument;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new MethodStep.MethodStep1(
                "hasValue",
                _argument is P.Eq eq
                    ? eq.Argument
                    : _argument);
        }
    }
}
