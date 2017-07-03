using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface ITypedGremlinQueryProvider : IHasModel, IHasTraversalSource
    {
        IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query);
    }
}