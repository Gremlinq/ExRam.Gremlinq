using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class GroovyIntegrationTests : QueryExecutionTestsBase, IClassFixture<GroovyGremlinServerContainerFixture>, ISourceFileNameProvider<GroovyIntegrationTests>
    {
        public GroovyIntegrationTests(GroovyGremlinServerContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
