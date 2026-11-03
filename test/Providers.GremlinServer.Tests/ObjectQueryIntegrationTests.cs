using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/>, every query cast to <see cref="object"/>, against Gremlin Server 3.7.7. Owns
    /// the snapshots of the facts that answer differently than the typed query; <see cref="ObjectQueryIntegrationTestsBase"/> names them.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class ObjectQueryIntegrationTests : ObjectQueryIntegrationTestsBase, IClassFixture<GremlinServerContainerFixture>, ISourceFileNameProvider<ObjectQueryIntegrationTests>
    {
        public ObjectQueryIntegrationTests(GremlinServerContainerFixture fixture) : base(fixture, new ObjectQueryExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
