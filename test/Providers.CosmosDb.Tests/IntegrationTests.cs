using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Fixtures;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<CosmosDbIntegrationTestFixture>
    {
        public IntegrationTests(CosmosDbIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
            fixture.Create().Wait();
        }
    }
}
