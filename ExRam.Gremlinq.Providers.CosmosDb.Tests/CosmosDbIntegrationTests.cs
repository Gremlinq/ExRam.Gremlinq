using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : IntegrationTests
    {
        public CosmosDbIntegrationTests() : base(g
            .UseCosmosDb( new Uri("wss://xyz.gremlin.cosmosdb.azure.com"), "db", "graph", "authkey"))
        {
        }
    }
}
