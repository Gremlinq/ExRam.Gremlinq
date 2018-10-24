using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class ResolutionStep : TerminalStep
    {
        private readonly IEnumerable<TerminalStep> _steps;

        public ResolutionStep(IEnumerable<TerminalStep> steps)
        {
            _steps = steps;
        }

        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            foreach (var step in _steps)
            {
                state = step.Serialize(stringBuilder, state);
            }

            return state;
        }
    }
}
