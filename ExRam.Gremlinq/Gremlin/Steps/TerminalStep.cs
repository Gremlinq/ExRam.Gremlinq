using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class TerminalStep : Step, IGroovySerializable
    {
        public abstract GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }
}
