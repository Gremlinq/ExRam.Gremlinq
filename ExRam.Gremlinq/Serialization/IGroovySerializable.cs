using System.Text;

namespace ExRam.Gremlinq
{
    public interface IGroovySerializable
    {
        GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }

    public interface ITopGroovySerializable : IGroovySerializable
    {
        GroovyExpressionState Serialize(StringBuilder stringBuilder);
    }
}