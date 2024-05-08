using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest]
    public sealed class IntegrationWithoutProjectionTests : QueryExecutionTest, IClassFixture<GremlinServerWithoutProjectionContainerFixture>
    {
        public IntegrationWithoutProjectionTests(GremlinServerWithoutProjectionContainerFixture fixture) : base(
            fixture,
            new JTokenExecutingVerifier())
        {
        }
    }
}
