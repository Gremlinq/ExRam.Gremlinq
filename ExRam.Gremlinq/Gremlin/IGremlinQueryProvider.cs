using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinQueryProvider
    {
        IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query);

        IGraphModel Model { get; }
    }
}