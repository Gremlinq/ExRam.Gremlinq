using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutor<in TSerializedQuery, out TQueryResult>
    {
        IAsyncEnumerable<TQueryResult> Execute(TSerializedQuery serializedQuery);
    }
}
