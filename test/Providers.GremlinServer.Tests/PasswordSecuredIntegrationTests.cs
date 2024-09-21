using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class PasswordSecuredIntegrationTests : QueryExecutionTest, IClassFixture<PasswordSecuredGremlinServerContainerFixture>
    {
        public PasswordSecuredIntegrationTests(PasswordSecuredGremlinServerContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }
    }
}
