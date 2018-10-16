using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class IdentifierStep : TerminalStep
    {
        public IdentifierStep(string identifier)
        {
            Identifier = identifier;
        }

        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state.AppendIdentifier(stringBuilder, Identifier);
        }

        public string Identifier { get; }
    }
}
