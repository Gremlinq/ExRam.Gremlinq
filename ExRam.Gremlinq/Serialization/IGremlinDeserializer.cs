using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinDeserializer
    {
        IAsyncEnumerable<TElement> Deserialize<TElement>(string rawData, IGraphModel model);
    }
}