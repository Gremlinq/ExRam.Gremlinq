using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExRam.Gremlinq
{
    public class ResolvedMethodStep : TerminalStep
    {
        private readonly string _name;
        private readonly IEnumerable<object> _parameters;

        public ResolvedMethodStep(string name) :this(name, Enumerable.Empty<object>())
        {

        }

        public ResolvedMethodStep(string name, IEnumerable<object> parameters)
        {
            _name = name;
            _parameters = parameters;
        }

        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state.AppendMethod(stringBuilder, _name, _parameters);
        }
    }
}
