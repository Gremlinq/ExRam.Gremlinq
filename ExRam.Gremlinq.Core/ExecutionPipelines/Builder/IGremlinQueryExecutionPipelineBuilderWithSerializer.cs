namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery>
    {
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult> UseExecutor<TExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> executor);
    }
}
