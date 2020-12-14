#if FALSE   //Maybe on CosmosDb emulator one day...
using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : QueryExecutionTest
    {
        public CosmosDbIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(env => env
                .UseCosmosDb(builder => builder
                    .At(new Uri("wss://xyz.gremlin.cosmosdb.azure.com"), "db", "graph")
                    .AuthenticateBy("authkey"))),
            testOutputHelper)
        {
        }
    }
}
#endif
