using System.Collections.Immutable;
using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class TerminalGremlinStep : GremlinStep, IGremlinSerializable
    {
        public TerminalGremlinStep(string name, params object[] parameters) : this(name, ImmutableList.Create(parameters))
        {

        }

        public TerminalGremlinStep(string name, IImmutableList<object> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }

        public GroovyExpressionBuilder Serialize(StringBuilder stringBuilder, GroovyExpressionBuilder builder)
        {
            return builder.AppendMethod(stringBuilder, this.Name, this.Parameters);
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}