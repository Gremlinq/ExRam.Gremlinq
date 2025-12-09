using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class StringIdIntegrationTests : QueryExecutionTest, IClassFixture<StringIdGremlinServerContainerFixture>
    {
        public StringIdIntegrationTests(StringIdGremlinServerContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }
    }
}
