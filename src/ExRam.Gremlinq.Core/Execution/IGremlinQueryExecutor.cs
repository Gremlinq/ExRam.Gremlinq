using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(Bytecode bytecode, IGremlinQueryEnvironment environment);
    }
}
