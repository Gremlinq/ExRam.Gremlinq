using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment);
    }
}
