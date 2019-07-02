namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithExecutor<out TExecutionResult>
    {
        IGremlinQueryExecutionPipeline UseDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
