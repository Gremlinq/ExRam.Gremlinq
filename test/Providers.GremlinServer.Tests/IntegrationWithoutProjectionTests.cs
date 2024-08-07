using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public class IntegrationWithoutProjectionTests : QueryExecutionTest, IClassFixture<GremlinServerWithoutProjectionContainerFixture>
    {
        public IntegrationWithoutProjectionTests(GremlinServerWithoutProjectionContainerFixture fixture) : base(
            fixture,
            new JTokenExecutingVerifier())
        {
        }
    }
}
