using ExRam.Gremlinq.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class IntegrationTests : Providers.Tests.IntegrationTests
    {
        public IntegrationTests() : base(g
            .WithCosmosDbRemote("xyz.gremlin.cosmosdb.azure.com", "db", "graph", "authkey"))
        {
        }
    }
}
