using System;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketProviderConfigurator<out TConfigurator> : IProviderConfigurator<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        TConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation);
    }
}
