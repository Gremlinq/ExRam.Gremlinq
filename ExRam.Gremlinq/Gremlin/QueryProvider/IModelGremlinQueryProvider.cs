using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider
    {
        IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query, IGremlinDeserializer serializer);
        
        IGraphModel Model { get; }
    }
}