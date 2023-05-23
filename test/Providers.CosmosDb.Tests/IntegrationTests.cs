using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public IntegrationTests(CosmosDbFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
            fixture.Create().Wait();
        }
    }
}
