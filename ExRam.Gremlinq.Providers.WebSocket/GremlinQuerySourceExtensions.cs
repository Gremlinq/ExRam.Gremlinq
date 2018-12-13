using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource WithRemote(this IGremlinQuerySource source, string hostname, int port = 8182, bool enableSsl = false, string username = null, string password = null)
        {
            return source
                .WithQueryProvider(new ClientGremlinQueryProvider(new GremlinServer(hostname, port, enableSsl, username, password)));
        }
    }
}
