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

        //TODO: Func will man nicht mehr.
        private sealed class FuncGremlinClientFactory : IGremlinClientFactory
        {
            private readonly Func<IGremlinQueryEnvironment, IGremlinClient> _factory;

            public FuncGremlinClientFactory(Func<IGremlinQueryEnvironment, IGremlinClient> factory)
            {
                _factory = factory;
            }

            public IGremlinClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> poolSettingsConfiguration)
            {
                //TODO!!
                throw new NotImplementedException();
            }

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation)
            {
                //TODO!!
                throw new NotImplementedException();
            }

            public IGremlinClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> optionsConfiguration)
            {
                //TODO!!!
                throw new NotImplementedException();
            }

            public IGremlinClient Create(IGremlinQueryEnvironment environment) => _factory(environment);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory Create(Func<IGremlinQueryEnvironment, IGremlinClient> factory) => new FuncGremlinClientFactory(factory);

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation) => Create((environment) => clientTransformation(clientFactory.Create(environment)));

        internal static IGremlinClientFactory Log(this IGremlinClientFactory clientFactory)
        {
            return Create((environment) => clientFactory
                .Create(environment)
                .Log(environment));
        }
    }
}
