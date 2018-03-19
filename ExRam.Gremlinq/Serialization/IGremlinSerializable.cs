namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        GroovyExpressionBuilder Serialize(GroovyExpressionBuilder builder);
    }
}