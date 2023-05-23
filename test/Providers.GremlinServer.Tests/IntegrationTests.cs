using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.GremlinServer.Tests.Fixtures;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerIntegrationTestFixture>
    {
        public IntegrationTests(GremlinServerIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
        }
    }
}
