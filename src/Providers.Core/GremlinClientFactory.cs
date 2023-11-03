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

            public DefaultGremlinClientFactory() : this(new GremlinServer(), _ => { })
            {

            }

            private DefaultGremlinClientFactory(GremlinServer server, Action<ClientWebSocketOptions> webSocketOptionsConfiguration)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new DefaultGremlinClientFactory(transformation(_server), _webSocketOptionsConfiguration);

            public IGremlinClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> optionsConfiguration) => new DefaultGremlinClientFactory(
                _server,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    optionsConfiguration(options);
                });

            public IGremlinClient Create(IGremlinQueryEnvironment environment, ConnectionPoolSettings connectionPoolSettings)
            {
                return new GremlinClient(
                    _server,
                    new DefaultMessageSerializer(environment
                        .ConfigureDeserializer(deserializer => deserializer
                            .Add(ObjectIdentityConverterFactory))),
                    connectionPoolSettings,
                    options => _webSocketOptionsConfiguration(options));
            }


        }

        //TODO: Func will man nicht mehr.
        private sealed class FuncGremlinClientFactory : IGremlinClientFactory
        {
            private readonly Func<IGremlinQueryEnvironment, ConnectionPoolSettings, IGremlinClient> _factory;

            public FuncGremlinClientFactory(Func<IGremlinQueryEnvironment, ConnectionPoolSettings, IGremlinClient> factory)
            {
                _factory = factory;
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

            public IGremlinClient Create(IGremlinQueryEnvironment environment, ConnectionPoolSettings connectionPoolSettings) => _factory(environment, connectionPoolSettings);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory Create(Func<IGremlinQueryEnvironment, ConnectionPoolSettings, IGremlinClient> factory) => new FuncGremlinClientFactory(factory);

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation) => Create((environment, poolSettings) => clientTransformation(clientFactory.Create(environment, poolSettings)));

        internal static IGremlinClientFactory Log(this IGremlinClientFactory clientFactory)
        {
            return Create((environment, poolSettings) => clientFactory
                .Create(environment, poolSettings)
                .Log(environment));
        }
    }
}
