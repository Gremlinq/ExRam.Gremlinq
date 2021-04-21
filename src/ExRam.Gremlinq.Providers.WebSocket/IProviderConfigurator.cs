using System;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IProviderConfigurator<out TConfigurator> : IGremlinQuerySourceTransformation
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        TConfigurator At(Uri uri);

        TConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation);
    }
}
