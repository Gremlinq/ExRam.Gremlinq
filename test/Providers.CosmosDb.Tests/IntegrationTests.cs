using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<Integration<CosmosDbFixture>>
    {
        public IntegrationTests(Integration<CosmosDbFixture> fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
            fixture.Inner.Create().Wait();
        }
    }
}
