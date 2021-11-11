using System;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : IGremlinQuerySourceTransformation
    {
        IWebSocketConfigurator At(Uri uri);

        IWebSocketConfigurator AuthenticateBy(string username, string password);

        IWebSocketConfigurator SetAlias(string alias);

        IWebSocketConfigurator ConfigureGremlinClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation);

        IWebSocketConfigurator ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation);

        IWebSocketConfigurator ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation);

        IWebSocketConfigurator ConfigureMessageSerializer(Func<IMessageSerializer, IMessageSerializer> transformation);
    }
}
