namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, out TExecutionResult>
    {
        IGremlinQueryExecutionPipeline UseDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
