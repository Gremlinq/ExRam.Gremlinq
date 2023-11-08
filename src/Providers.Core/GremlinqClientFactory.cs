using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinqClientFactory
    {
        private sealed class GremlinqClientFactoryImpl : IGremlinqClientFactory
        {
            public static readonly GremlinqClientFactoryImpl LocalHost = new (new GremlinServer(), _ => { }, _ => { });

            private readonly GremlinServer _server;
            private readonly Action<ClientWebSocketOptions> _webSocketOptionsConfiguration;
            private readonly Action<ConnectionPoolSettings> _poolSettingsConfiguration;

            private GremlinqClientFactoryImpl(GremlinServer server, Action<ClientWebSocketOptions> webSocketOptionsConfiguration, Action<ConnectionPoolSettings> poolSettingsConfiguration)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
                _poolSettingsConfiguration = poolSettingsConfiguration;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new GremlinqClientFactoryImpl(transformation(_server), _webSocketOptionsConfiguration, _poolSettingsConfiguration);

            public IGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration) => new GremlinqClientFactoryImpl(
                _server,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    configuration(options);
                },
                _poolSettingsConfiguration);

            public IGremlinqClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> configuration) => new GremlinqClientFactoryImpl(
                _server,
                _webSocketOptionsConfiguration,
                settings =>
                {
                    _poolSettingsConfiguration(settings);
                    configuration(settings);
                });

            public IGremlinqClient Create(IGremlinQueryEnvironment environment)
            {
                var poolSettings = new ConnectionPoolSettings();

                _poolSettingsConfiguration(poolSettings);

                return new WebSocketGremlinqClientPool(_server, poolSettings, _webSocketOptionsConfiguration, environment);
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

            public IGremlinqClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> poolSettingsConfiguration) => new ConfigureClientGremlinClientFactory(_baseFactory.ConfigureConnectionPool(poolSettingsConfiguration), _clientTransformation);

            public IGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new ConfigureClientGremlinClientFactory(_baseFactory.ConfigureServer(transformation), _clientTransformation);

            public IGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> optionsConfiguration) => new ConfigureClientGremlinClientFactory(_baseFactory.ConfigureWebSocketOptions(optionsConfiguration), _clientTransformation);

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => _clientTransformation(_baseFactory.Create(environment), environment);
        }

        internal static readonly IGremlinqClientFactory LocalHost = GremlinqClientFactoryImpl.LocalHost;

        public static IGremlinqClientFactory ConfigureClient(this IGremlinqClientFactory clientFactory, Func<IGremlinqClient, IGremlinqClient> clientTransformation) => new ConfigureClientGremlinClientFactory(clientFactory, (client, _) => clientTransformation(client));

        internal static IGremlinqClientFactory Log(this IGremlinqClientFactory clientFactory) => new ConfigureClientGremlinClientFactory(clientFactory, (client, environment) => client.Log(environment));
    }
}
