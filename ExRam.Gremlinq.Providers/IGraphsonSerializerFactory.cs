using ExRam.Gremlinq.Core;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Providers
{
    public interface IGraphsonSerializerFactory
    {
        JsonSerializer Get(IGraphModel model);
    }
}
