using ExRam.Gremlinq.Core;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Providers
{
    public interface IGraphsonDeserializerFactory
    {
        JsonSerializer Get(IGraphModel model);
    }
}
