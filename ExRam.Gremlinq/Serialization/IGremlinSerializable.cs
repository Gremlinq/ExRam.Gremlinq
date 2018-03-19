using System.Text;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        GroovyExpressionBuilder Serialize(StringBuilder stringBuilder, GroovyExpressionBuilder builder);
    }
}