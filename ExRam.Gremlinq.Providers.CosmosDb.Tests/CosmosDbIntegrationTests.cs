using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : IntegrationTests
    {
        public CosmosDbIntegrationTests() : base(g.ConfigureEnvironment(env => env
            .UseCosmosDb(builder => builder
                .At(new Uri("wss://xyz.gremlin.cosmosdb.azure.com"), "db", "graph")
                .AuthenticateBy("authkey"))))
        {
        }
    }
}
