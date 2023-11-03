using System;
using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientFactory
    {
        private sealed class DefaultGremlinClientFactory : IGremlinClientFactory
        {
            private sealed class DefaultMessageSerializer : IMessageSerializer
            {
                private readonly IGremlinQueryEnvironment _environment;

                public DefaultMessageSerializer(IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                }

                public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage, CancellationToken ct) => _environment.Serializer
                    .TransformTo<byte[]>()
                    .From(requestMessage, _environment);

                public async Task<ResponseMessage<List<object>>?> DeserializeMessageAsync(byte[] message, CancellationToken ct) => _environment.Deserializer
                    .TryTransformTo<ResponseMessage<List<object>>>()
                    .From(message, _environment);
            }

            private static readonly IConverterFactory ObjectIdentityConverterFactory = ConverterFactory.Create<object, object>((token, _, _, _) => token);

            private readonly GremlinServer _server;
            private readonly Action<ClientWebSocketOptions> _webSocketOptionsConfiguration;
            private readonly Action<ConnectionPoolSettings> _poolSettingsConfiguration;

            public DefaultGremlinClientFactory() : this(new GremlinServer(), _ => { }, _ => { })
            {

            }

            private DefaultGremlinClientFactory(GremlinServer server, Action<ClientWebSocketOptions> webSocketOptionsConfiguration, Action<ConnectionPoolSettings> poolSettingsConfiguration)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
                _poolSettingsConfiguration = poolSettingsConfiguration;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new DefaultGremlinClientFactory(transformation(_server), _webSocketOptionsConfiguration, _poolSettingsConfiguration);

            public IGremlinClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> optionsConfiguration) => new DefaultGremlinClientFactory(
                _server,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    optionsConfiguration(options);
                },
                _poolSettingsConfiguration);

            public IGremlinClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> poolSettingsConfiguration) => new DefaultGremlinClientFactory(
                _server,
                _webSocketOptionsConfiguration,
                settings =>
                {
                    _poolSettingsConfiguration(settings);
                    poolSettingsConfiguration(settings);
                });

            public IGremlinClient Create(IGremlinQueryEnvironment environment)
            {
                var poolSettings = new ConnectionPoolSettings();

                _poolSettingsConfiguration(poolSettings);

                return new GremlinClient(
                    _server,
                    new DefaultMessageSerializer(environment
                        .ConfigureDeserializer(deserializer => deserializer
                            .Add(ObjectIdentityConverterFactory))),
                    poolSettings,
                    options => _webSocketOptionsConfiguration(options));
            }
        }

        private sealed class ConfigureClientGremlinClientFactory : IGremlinClientFactory
        {
            private readonly IGremlinClientFactory _baseFactory;
            private readonly Func<IGremlinClient, IGremlinQueryEnvironment, IGremlinClient> _clientTransformation;

            public ConfigureClientGremlinClientFactory(IGremlinClientFactory baseFactory, Func<IGremlinClient, IGremlinQueryEnvironment, IGremlinClient> clientTransformation)
            {
                _baseFactory = baseFactory;
                _clientTransformation = clientTransformation;
            }

            public IGremlinClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> poolSettingsConfiguration) => new ConfigureClientGremlinClientFactory(_baseFactory.ConfigureConnectionPool(poolSettingsConfiguration), _clientTransformation);

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new ConfigureClientGremlinClientFactory(_baseFactory.ConfigureServer(transformation), _clientTransformation);

            public IGremlinClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> optionsConfiguration) => new ConfigureClientGremlinClientFactory(_baseFactory.ConfigureWebSocketOptions(optionsConfiguration), _clientTransformation);

            public IGremlinClient Create(IGremlinQueryEnvironment environment) => _clientTransformation(_baseFactory.Create(environment), environment);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation) => new ConfigureClientGremlinClientFactory(clientFactory, (client, _) => clientTransformation(client));

        internal static IGremlinClientFactory Log(this IGremlinClientFactory clientFactory) => new ConfigureClientGremlinClientFactory(clientFactory, (client, environment) => client.Log(environment));
    }
}
