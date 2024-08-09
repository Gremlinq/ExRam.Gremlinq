using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class GroovyIntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public GroovyIntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new JTokenExecutingVerifier())
        {
        }
    }
}
