using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IGremlinQueryEnvironmentBuilder
    {
        IGremlinQueryEnvironment Build();
    }
}