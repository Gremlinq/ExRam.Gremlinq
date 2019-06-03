namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilder
    {
        IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery> AddSerializer<TSerializedQuery>(IGremlinQuerySerializer<TSerializedQuery> serializer);
    }
}
