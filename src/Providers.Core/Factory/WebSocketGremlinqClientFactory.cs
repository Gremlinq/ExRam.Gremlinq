using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class WebSocketGremlinqClientFactory
    {
        private sealed class WebSocketGremlinqClientFactoryImpl : IWebSocketGremlinqClientFactory
        {
            public static readonly WebSocketGremlinqClientFactoryImpl LocalHost = new(new GremlinServer(), _ => { });

            private readonly GremlinServer _server;
            private readonly Action<ClientWebSocketOptions> _webSocketOptionsConfiguration;

            private WebSocketGremlinqClientFactoryImpl(GremlinServer server, Action<ClientWebSocketOptions> webSocketOptionsConfiguration)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IWebSocketGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new WebSocketGremlinqClientFactoryImpl(transformation(_server), _webSocketOptionsConfiguration);

            public IWebSocketGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration) => new WebSocketGremlinqClientFactoryImpl(
                _server,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    configuration(options);
                });

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new WebSocketGremlinqClient(_server, _webSocketOptionsConfiguration, environment);
        }

        public static readonly IWebSocketGremlinqClientFactory LocalHost = WebSocketGremlinqClientFactoryImpl.LocalHost;
    }
}
