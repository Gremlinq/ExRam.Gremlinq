using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IConfigurableGremlinQuerySource WithCosmosDbRemote(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443)
        {
            return source.WithExecutor(
                new WebSocketGremlinQueryExecutor<CosmosDbGroovyGremlinQueryElementVisitor>(
                    new CosmosDbGremlinClient(
                        new CosmosDbGremlinServer(hostname, database, graphName, authKey, port)),
                    new CosmosDbGraphsonSerializerFactory(),
                    source.Logger));
        }
    }
}
