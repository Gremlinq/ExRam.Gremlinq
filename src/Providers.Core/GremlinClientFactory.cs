using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientFactory
    {
        private sealed class DefaultGremlinClientFactory : IGremlinClientFactory
        {
            public IGremlinClient Create(IGremlinQueryEnvironment _, GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration) => new GremlinClient(
                gremlinServer,
                messageSerializer,
                connectionPoolSettings,
                webSocketConfiguration);
        }

        private sealed class FuncGremlinClientFactory : IGremlinClientFactory
        {
            private readonly Func<IGremlinQueryEnvironment, GremlinServer, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> _factory;

            public FuncGremlinClientFactory(Func<IGremlinQueryEnvironment, GremlinServer, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> factory)
            {
                _factory = factory;
            }

            public IGremlinClient Create(IGremlinQueryEnvironment environment, GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration) => _factory(environment, gremlinServer, messageSerializer, connectionPoolSettings, webSocketConfiguration);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory Create(Func<IGremlinQueryEnvironment, GremlinServer, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> factory) => new FuncGremlinClientFactory(factory);

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation) => Create((environment, server, serializer, poolSettings, optionsTransformation) => clientTransformation(clientFactory.Create(environment, server, serializer, poolSettings, optionsTransformation)));

        internal static IGremlinClientFactory Log(this IGremlinClientFactory clientFactory)
        {
            return Create((environment, server, serializer, poolSettings, optionsTransformation) => clientFactory
                .Create(environment, server, serializer, poolSettings, optionsTransformation)
                .Log(environment));
        }
    }
}
