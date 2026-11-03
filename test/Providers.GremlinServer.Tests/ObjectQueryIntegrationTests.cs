using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class ObjectQueryIntegrationTests : QueryExecutionTestsBase, IClassFixture<GremlinServerContainerFixture>, ISourceFileNameProvider<ObjectQueryIntegrationTests>
    {
        public ObjectQueryIntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new ObjectQueryExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
