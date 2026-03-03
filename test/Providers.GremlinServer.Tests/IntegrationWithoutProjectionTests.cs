using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class IntegrationWithoutProjectionTests : QueryExecutionTest, IClassFixture<GremlinServerWithoutProjectionContainerFixture>
    {
        public IntegrationWithoutProjectionTests(GremlinServerWithoutProjectionContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }
    }
}
