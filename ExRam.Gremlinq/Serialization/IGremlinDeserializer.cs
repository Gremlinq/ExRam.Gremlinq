using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    public interface IGremlinDeserializer
    {
        IAsyncEnumerable<TElement> Deserialize<TElement>(JToken rawData, IGraphModel model);
    }
}