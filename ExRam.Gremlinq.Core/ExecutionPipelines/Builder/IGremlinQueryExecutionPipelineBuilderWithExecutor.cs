namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult>
    {
        IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> UseDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
