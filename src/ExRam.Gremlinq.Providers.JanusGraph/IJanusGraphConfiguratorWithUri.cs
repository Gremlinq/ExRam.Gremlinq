using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfiguratorWithUri : IGremlinQuerySourceTransformation
    {
        IGremlinQuerySourceTransformation ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation);
    }
}
