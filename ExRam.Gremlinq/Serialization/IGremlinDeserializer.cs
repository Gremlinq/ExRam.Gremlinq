using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinDeserializer
    {
        IAsyncEnumerable<T> Deserialize<T>(string rawData, IGraphModel model);
    }
}