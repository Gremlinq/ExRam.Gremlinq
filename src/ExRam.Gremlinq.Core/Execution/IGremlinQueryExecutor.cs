using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment);
    }
}
