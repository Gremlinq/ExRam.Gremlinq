namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionResultDeserializerFactory<in TExecutionResult>
    {
        IGremlinQueryExecutionResultDeserializer<TExecutionResult> Get(IGremlinQueryEnvironment environment);
    }
}
