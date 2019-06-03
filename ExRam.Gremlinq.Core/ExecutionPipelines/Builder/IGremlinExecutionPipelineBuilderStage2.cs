namespace ExRam.Gremlinq.Core
{
    public interface IGremlinExecutionPipelineBuilderStage2<out TSerializedQuery>
    {
        IGremlinExecutionPipelineBuilderStage3<TExecutionResult> AddExecutor<TExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> executor);
    }
}
