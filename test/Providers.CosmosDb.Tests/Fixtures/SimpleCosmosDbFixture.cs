using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests.Fixtures
{
    public sealed class SimpleCosmosDbFixture : GremlinqTestFixture
    {
        public SimpleCosmosDbFixture() : base(g
            .UseCosmosDb(_ => _
                .At("ws://localhost", "", "")
                .AuthenticateBy("")
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .AddFakePartitionKey()))
        {
        }
    }
}
