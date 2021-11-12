using System;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : IGremlinQuerySourceTransformation
    {
        IWebSocketConfigurator At(Uri uri);

        IWebSocketConfigurator AuthenticateBy(string username, string password);

        IWebSocketConfigurator SetAlias(string alias);

        IWebSocketConfigurator ConfigureGremlinClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation);
    }
}
