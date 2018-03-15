using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider : IHasModel, IHasTraversalSource
    {
        IAsyncEnumerable<string> Execute<TElement>(IGremlinQuery<TElement> query);
    }
}