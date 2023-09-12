using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class CosmosDbFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IConfigurableGremlinQuerySource g) => g
            .UseCosmosDb(builder => builder
                .AtLocalhost("db", "graph")
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .AddFakePartitionKey());
    }
}
