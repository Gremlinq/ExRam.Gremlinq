namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderStage3<out TExecutionResult>
    {
        IGremlinQueryExecutionPipeline AddDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
