using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(BytecodeGremlinQuery serializedQuery, IGremlinQueryEnvironment environment);
    }
}
