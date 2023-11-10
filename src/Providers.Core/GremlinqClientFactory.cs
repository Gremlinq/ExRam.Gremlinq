using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinqClientFactory
    {
        private sealed class GremlinqClientFactoryImpl : IWebSocketGremlinqClientFactory
        {
            public static readonly GremlinqClientFactoryImpl LocalHost = new (new GremlinServer(), _ => { });

            private readonly GremlinServer _server;
            private readonly Action<ClientWebSocketOptions> _webSocketOptionsConfiguration;

            private GremlinqClientFactoryImpl(GremlinServer server, Action<ClientWebSocketOptions> webSocketOptionsConfiguration)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IWebSocketGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new GremlinqClientFactoryImpl(transformation(_server), _webSocketOptionsConfiguration);

            public IWebSocketGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration) => new GremlinqClientFactoryImpl(
                _server,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    configuration(options);
                });

            public IGremlinqClient Create(IGremlinQueryEnvironment environment)
            {
                return new WebSocketGremlinqClient(_server, _webSocketOptionsConfiguration, environment);
            }
        }

        private sealed class ConfigureClientGremlinClientFactory : IGremlinqClientFactory
        {
            private readonly IGremlinqClientFactory _baseFactory;
            private readonly Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> _clientTransformation;

            public ConfigureClientGremlinClientFactory(IGremlinqClientFactory baseFactory, Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation)
            {
                _baseFactory = baseFactory;
                _clientTransformation = clientTransformation;
            }

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => _clientTransformation(_baseFactory.Create(environment), environment);
        }

        internal static readonly IWebSocketGremlinqClientFactory LocalHost = GremlinqClientFactoryImpl.LocalHost;

        public static IGremlinqClientFactory ConfigureClient(this IGremlinqClientFactory clientFactory, Func<IGremlinqClient, IGremlinqClient> clientTransformation) => new ConfigureClientGremlinClientFactory(clientFactory, (client, _) => clientTransformation(client));

        public static IPoolGremlinqClientFactory<TBaseFactory> Pool<TBaseFactory>(this TBaseFactory baseFactory)
            where TBaseFactory : IGremlinqClientFactory => new PoolGremlinqClientFactory<TBaseFactory>(baseFactory);

        internal static IGremlinqClientFactory Log(this IGremlinqClientFactory clientFactory) => new ConfigureClientGremlinClientFactory(clientFactory, (client, environment) => client.Log(environment));
    }
}
