using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinQueryProvider : IHasModel, IHasTraversalSource
    {
        IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query);
    }
}