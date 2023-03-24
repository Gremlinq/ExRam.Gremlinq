using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientFactory
    {
        private sealed class DefaultGremlinClientFactory : IGremlinClientFactory
        {
            public IGremlinClient Create(IGremlinQueryEnvironment _, GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null) => new GremlinClient(
                gremlinServer,
                messageSerializer,
                connectionPoolSettings,
                webSocketConfiguration,
                sessionId);
        }

        private sealed class FuncGremlinClientFactory : IGremlinClientFactory
        {
            private readonly Func<IGremlinQueryEnvironment, GremlinServer, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, string?, IGremlinClient> _factory;

            public FuncGremlinClientFactory(Func<IGremlinQueryEnvironment, GremlinServer, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, string?, IGremlinClient> factory)
            {
                _factory = factory;
            }

            public IGremlinClient Create(IGremlinQueryEnvironment environment, GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null) => _factory(environment, gremlinServer, messageSerializer, connectionPoolSettings, webSocketConfiguration, sessionId);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory Create(Func<IGremlinQueryEnvironment, GremlinServer, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, string?, IGremlinClient> factory) => new FuncGremlinClientFactory(factory);

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation)
        {
            return Create((environment, server, serializer, poolSettings, optionsTransformation, sessionId) => clientTransformation(clientFactory.Create(environment, server, serializer, poolSettings, optionsTransformation, sessionId)));
        }
    }
}
