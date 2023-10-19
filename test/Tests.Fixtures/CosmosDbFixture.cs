using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class CosmosDbFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
            .UseCosmosDb<Vertex, Edge>(
                builder => builder
                    .AtLocalhost("db", "graph")
                    .WithPartitionKey(x => x.Label!)
                    .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .AddFakePartitionKey());
    }
}
