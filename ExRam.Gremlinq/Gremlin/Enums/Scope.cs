using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class Scope : IGroovySerializable
    {
        public static readonly Scope Local = new Scope("local");
        public static readonly Scope Global = new Scope("global");

        private readonly string _name;

        private Scope(string name)
        {
            _name = name;
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state
                .AppendIdentifier(stringBuilder, nameof(Scope))
                .AppendField(stringBuilder, _name);
        }
    }
}