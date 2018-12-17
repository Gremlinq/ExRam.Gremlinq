using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinQueryExecutor
    {
        bool SupportsElementType(Type type);
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}
