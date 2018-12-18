using System.Collections.Generic;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IConfigurableGremlinQuerySource WithCosmosDbRemote(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443, bool enableSsl = true)
        {
            return source.WithExecutor(
                new WebSocketGremlinQueryExecutor(
                    new CosmosDbGremlinClient(
                        new CosmosDbGremlinServer(hostname, database, graphName, authKey, port, enableSsl)),
                    new StringGremlinQuerySerializer<CosmosDbGroovyGremlinQueryElementVisitor>(),
                    new CosmosDbGraphsonSerializerFactory()));
        }
    }
}
