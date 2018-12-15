using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Serialization;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource WithRemote(this IGremlinQuerySource source, string hostname, GraphsonVersion graphsonVersion, int port = 8182, bool enableSsl = false, string username = null, string password = null)
        {
            return source.WithRemote(
                new GremlinServer(hostname, port, enableSsl, username, password),
                graphsonVersion);
        }

        public static IGremlinQuerySource WithRemote(this IGremlinQuerySource source, GremlinServer server, GraphsonVersion graphsonVersion)
        {
            return source.WithQueryProvider(
                new ClientGremlinQueryProvider(
                    new GremlinClientEx(
                        server,
                        graphsonVersion),
                    new StringGremlinQuerySerializer<GroovyGremlinQueryElementVisitor>()));
        }
    }
}
