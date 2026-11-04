using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class LoggingIntegrationTests : QueryExecutionTestsBase, IClassFixture<LoggingGremlinServerContainerFixture>
    {
        public LoggingIntegrationTests(LoggingGremlinServerContainerFixture fixture) : base(
            fixture,
            new LoggingVerifier())
        {
        }
    }
}
