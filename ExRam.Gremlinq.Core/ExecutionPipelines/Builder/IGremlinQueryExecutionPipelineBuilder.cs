namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilder
    {
        IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery> UseSerializer<TSerializedQuery>(IGremlinQuerySerializer<TSerializedQuery> serializer);
    }
}
