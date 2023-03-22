using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(BytecodeGremlinQuery query, IGremlinQueryEnvironment environment);
    }
}
