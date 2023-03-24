namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment);
    }
}
