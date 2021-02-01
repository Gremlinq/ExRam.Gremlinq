//#if FALSE   //Maybe on CosmosDb emulator one day...
using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : QueryIntegrationTest
    {
        public CosmosDbIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(env => env
                .AddFakePartitionKey()
                .UseCosmosDb(builder => builder
                    .At(new Uri("ws://localhost:8901"), "db", "graph")
                    .AuthenticateBy("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))),
            testOutputHelper)
        {
        }
    }
}
//#endif
