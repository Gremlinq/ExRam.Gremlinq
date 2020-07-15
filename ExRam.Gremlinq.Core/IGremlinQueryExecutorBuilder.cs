namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutorBuilder
    {
        IGremlinQueryExecutor Build();

        IGremlinQueryEnvironment Environment { get; }
    }
}
