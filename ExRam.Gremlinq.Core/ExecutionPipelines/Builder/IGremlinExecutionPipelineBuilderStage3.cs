namespace ExRam.Gremlinq.Core
{
    public interface IGremlinExecutionPipelineBuilderStage3<out TExecutionResult>
    {
        IGremlinQueryExecutionPipeline AddDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
