namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithSerializer<out TSerializedQuery>
    {
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TExecutionResult> UseExecutor<TExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> executor);
    }
}
