namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context);
    }
}
