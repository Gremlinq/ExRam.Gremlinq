namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderStage2<out TSerializedQuery>
    {
        IGremlinQueryExecutionPipelineBuilderStage3<TExecutionResult> AddExecutor<TExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> executor);
    }
}
