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

            public IGremlinClient Create(IGremlinQueryEnvironment environment, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration)
            {
                return new GremlinClient(
                    _server,
                    new DefaultMessageSerializer(environment
                        .ConfigureDeserializer(deserializer => deserializer
                            .Add(ObjectIdentityConverterFactory))),
                    connectionPoolSettings,
                    webSocketConfiguration);
            }
        }

        private sealed class FuncGremlinClientFactory : IGremlinClientFactory
        {
            private readonly Func<IGremlinQueryEnvironment, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> _factory;

            public FuncGremlinClientFactory(Func<IGremlinQueryEnvironment, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> factory)
            {
                _factory = factory;
            }

            public IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation)
            {
                //TODO!!
                throw new NotImplementedException();
            }

            public IGremlinClient Create(IGremlinQueryEnvironment environment, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration) => _factory(environment, connectionPoolSettings, webSocketConfiguration);
        }

        public static readonly IGremlinClientFactory Default = new DefaultGremlinClientFactory();

        public static IGremlinClientFactory Create(Func<IGremlinQueryEnvironment, ConnectionPoolSettings, Action<ClientWebSocketOptions>, IGremlinClient> factory) => new FuncGremlinClientFactory(factory);

        public static IGremlinClientFactory ConfigureClient(this IGremlinClientFactory clientFactory, Func<IGremlinClient, IGremlinClient> clientTransformation) => Create((environment, poolSettings, optionsTransformation) => clientTransformation(clientFactory.Create(environment, poolSettings, optionsTransformation)));

        internal static IGremlinClientFactory Log(this IGremlinClientFactory clientFactory)
        {
            return Create((environment, poolSettings, optionsTransformation) => clientFactory
                .Create(environment, poolSettings, optionsTransformation)
                .Log(environment));
        }
    }
}
