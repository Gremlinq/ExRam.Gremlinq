namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithExecutor<out TExecutionResult>
    {
        IGremlinQueryExecutionPipeline AddDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
