using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinQueryExecutor
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}
