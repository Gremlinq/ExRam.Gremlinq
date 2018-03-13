using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface ITypedGremlinQueryProvider : IHasModel, IHasTraversalSource
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}