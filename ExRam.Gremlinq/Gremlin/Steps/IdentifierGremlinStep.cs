using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class IdentifierGremlinStep : TerminalGremlinStep
    {
        public IdentifierGremlinStep(string identifier)
        {
            this.Identifier = identifier;
        }

        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state.AppendIdentifier(stringBuilder, this.Identifier);
        }

        public string Identifier { get; }
    }
}