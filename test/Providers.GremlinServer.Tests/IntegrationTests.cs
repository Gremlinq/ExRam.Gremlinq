using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<IntegrationGremlinServerFixture>
    {
        public IntegrationTests(IntegrationGremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
        }
    }
}
