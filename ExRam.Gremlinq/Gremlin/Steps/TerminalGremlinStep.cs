using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class TerminalGremlinStep : GremlinStep, IGroovySerializable
    {
        public abstract GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }
}