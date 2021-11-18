using System;
using ExRam.Gremlinq.Core;
using _GremlinServer = Gremlin.Net.Driver.GremlinServer;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : IGremlinQuerySourceTransformation
    {
        IWebSocketConfigurator ConfigureAlias(Func<string, string> transformation);

        IWebSocketConfigurator ConfigureServer(Func<_GremlinServer, _GremlinServer> transformation);

        IWebSocketConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation);
    }
}
