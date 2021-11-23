using _GremlinServer = Gremlin.Net.Driver.GremlinServer;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinServerExtensions
    {
        public static _GremlinServer WithHost(this _GremlinServer server, string host) => new _GremlinServer(
            host,
            server.Uri.Port,
            server.IsSslEnabled(),
            server.Username,
            server.Password);

        public static _GremlinServer WithPort(this _GremlinServer server, int port) => new _GremlinServer(
            server.Uri.Host,
            port,
            server.IsSslEnabled(),
            server.Username,
            server.Password);

        public static _GremlinServer WithUsername(this _GremlinServer server, string username) => new _GremlinServer(
            server.Uri.Host,
            server.Uri.Port,
            server.IsSslEnabled(),
            username,
            server.Password);

        public static _GremlinServer WithPassword(this _GremlinServer server, string password) => new _GremlinServer(
            server.Uri.Host,
            server.Uri.Port,
            server.IsSslEnabled(),
            server.Username,
            password);

        public static _GremlinServer WithSslEnabled(this _GremlinServer server, bool sslEnabled) => new _GremlinServer(
            server.Uri.Host,
            server.Uri.Port,
            sslEnabled,
            server.Username,
            server.Password);

        private static bool IsSslEnabled(this _GremlinServer server) => server.Uri.Scheme is "wss" or "https";
    }
}
