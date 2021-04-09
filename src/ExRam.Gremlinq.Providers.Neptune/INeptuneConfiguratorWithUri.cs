using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfiguratorWithUri : IGremlinQuerySourceTransformation
    {
        IGremlinQuerySourceTransformation ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation);
    }
}
