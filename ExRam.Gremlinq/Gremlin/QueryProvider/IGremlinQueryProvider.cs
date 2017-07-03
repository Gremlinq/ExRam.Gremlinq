using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinQueryProvider : IHasModel
    {
        IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query);
    }
}