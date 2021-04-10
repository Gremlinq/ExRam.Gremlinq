using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfiguratorWithUri : IGremlinQuerySourceTransformation
    {
        INeptuneConfiguratorWithUri UseElasticSearch(Uri endPoint);

        INeptuneConfiguratorWithUri ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation);
    }
}
