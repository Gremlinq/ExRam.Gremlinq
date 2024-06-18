using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Windows", "Linux")]
    public sealed class IntegrationAndDeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public IntegrationAndDeserializationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }
    }
}
