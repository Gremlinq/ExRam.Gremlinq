using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}
