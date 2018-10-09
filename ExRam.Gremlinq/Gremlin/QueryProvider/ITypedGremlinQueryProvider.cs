using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface ITypedGremlinQueryProvider
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}