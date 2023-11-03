using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientFactory
    {
        private sealed class DefaultGremlinClientFactory : IGremlinClientFactory
        {
            private readonly GremlinServer _server;

            public DefaultGremlinClientFactory() : this(new GremlinServer())
            {

            }

            private DefaultGremlinClientFactory(GremlinServer server)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
            }

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new DefaultGremlinClientFactory(transformation(_server));

            public IGremlinClient Create(IGremlinQueryEnvironment _, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration)
            {
                return new GremlinClient(
                    _server,
                    messageSerializer,
                    connectionPoolSettings,
                    webSocketConfiguration);
            }
        }

        private sealed class FuncGremlinClientFactory : IGremlinClientFactory
        {
            private readonly Func<IGremlinQueryEnvironment, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> _factory;

            public FuncGremlinClientFactory(Func<IGremlinQueryEnvironment, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> factory)
            {
                _factory = factory;
            }

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation)
            {
                //TODO!!
                throw new NotImplementedException();
            }

            public IGremlinClient Create(IGremlinQueryEnvironment environment, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration) => _factory(environment, messageSerializer, connectionPoolSettings, webSocketConfiguration);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory Create(Func<IGremlinQueryEnvironment, IMessageSerializer, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> factory) => new FuncGremlinClientFactory(factory);

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation) => Create((environment, serializer, poolSettings, optionsTransformation) => clientTransformation(clientFactory.Create(environment, serializer, poolSettings, optionsTransformation)));

        internal static IGremlinClientFactory Log(this IGremlinClientFactory clientFactory)
        {
            return Create((environment, serializer, poolSettings, optionsTransformation) => clientFactory
                .Create(environment, serializer, poolSettings, optionsTransformation)
                .Log(environment));
        }
    }
}
