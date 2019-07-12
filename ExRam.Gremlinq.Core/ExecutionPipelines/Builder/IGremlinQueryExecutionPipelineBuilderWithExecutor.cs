namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult>
    {
        IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> UseDeserializer(IGremlinQueryExecutionResultDeserializer<TExecutionResult> deserializer);
    }
}
