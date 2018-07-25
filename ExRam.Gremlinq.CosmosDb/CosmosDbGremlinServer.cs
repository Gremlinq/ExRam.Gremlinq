using Gremlin.Net.Driver;
using Microsoft.Extensions.Options;

namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbGremlinServer : GremlinServer
    {
        public CosmosDbGremlinServer(IOptions<CosmosDbGraphConfiguration> configuration) : this(configuration.Value)
        {

        }

        private CosmosDbGremlinServer(CosmosDbGraphConfiguration configuration) : base(
            configuration.Hostname,
            configuration.Port,
            configuration.EnableSsl,
            $"/dbs/{configuration.Database}/colls/{configuration.GraphName}",
            configuration.AuthKey)
        {

        }
    }
}