namespace ExRam.Gremlinq.Core
{
    public interface IGremlinExecutionPipelineBuilderStage1
    {
        IGremlinExecutionPipelineBuilderStage2<TSerializedQuery> AddSerializer<TSerializedQuery>(IGremlinQuerySerializer<TSerializedQuery> serializer);
    }
}
