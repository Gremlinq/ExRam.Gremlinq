using System.Collections.Immutable;

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

        public GroovyExpressionBuilder Serialize(GroovyExpressionBuilder builder)
        {
            return builder.AppendMethod(this.Name, this.Parameters);
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}