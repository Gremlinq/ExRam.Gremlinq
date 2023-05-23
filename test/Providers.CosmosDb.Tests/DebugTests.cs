using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Fixtures;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<SimpleCosmosDbFixture>
    {
        public DebugTests(SimpleCosmosDbFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DebugGremlinQueryVerifier(),
            testOutputHelper)
        {
        }
    }
}
