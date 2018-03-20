using System.Collections.Immutable;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class TerminalGremlinStep : GremlinStep, IGroovySerializable
    {
        public abstract GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }

    public sealed class MethodGremlinStep : TerminalGremlinStep
    {
        public MethodGremlinStep(string name, params object[] parameters) : this(name, ImmutableList.Create(parameters))
        {

        }

        public MethodGremlinStep(string name, IImmutableList<object> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }

        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state.AppendMethod(stringBuilder, this.Name, this.Parameters);
        }

        public string Name { get; }
        public IImmutableList<object> Parameters { get; }
    }
}