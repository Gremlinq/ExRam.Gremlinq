namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithSerializer<out TSerializedQuery>
    {
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TExecutionResult> AddExecutor<TExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> executor);
    }
}
