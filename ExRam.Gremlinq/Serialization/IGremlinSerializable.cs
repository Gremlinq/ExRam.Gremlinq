using System.Text;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }
}