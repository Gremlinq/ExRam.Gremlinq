using System.Collections.Generic;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Execution
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<object> Execute(ISerializedQuery serializedQuery, IGremlinQueryEnvironment environment);
    }
}
