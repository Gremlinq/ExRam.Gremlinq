namespace ExRam.Gremlinq.Core
{
    public interface IGremlinExecutionPipelineBuilder
    {
        IGremlinExecutionPipelineBuilderStage2<TSerializedQuery> AddSerializer<TSerializedQuery>(IGremlinQuerySerializer<TSerializedQuery> serializer);
    }
}
