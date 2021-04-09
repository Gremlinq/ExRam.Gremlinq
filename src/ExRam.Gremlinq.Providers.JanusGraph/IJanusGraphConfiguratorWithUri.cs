using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfiguratorWithUri : IGremlinQueryEnvironmentTransformation
    {
        IGremlinQueryEnvironmentTransformation ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation);
    }
}
