using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class IsStep : NonTerminalStep
    {
        private readonly object _argument;

        public IsStep(object argument)
        {
            _argument = argument;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new MethodStep(
                "is",
                _argument is P.Eq eq
                    ? eq.Argument
                    : _argument);
        }
    }
}