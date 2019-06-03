namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilder
    {
        IGremlinQueryExecutionPipelineBuilderStage2<TSerializedQuery> AddSerializer<TSerializedQuery>(IGremlinQuerySerializer<TSerializedQuery> serializer);
    }
}
