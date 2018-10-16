using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IQueryProvider
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}
