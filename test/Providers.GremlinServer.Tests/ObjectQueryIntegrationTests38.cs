using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/>, every query cast to <see cref="object"/>, against Gremlin Server 3.8.2. Owns no
    /// snapshots: <see cref="ObjectQueryIntegrationTests38Base"/> redirects every fact.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class ObjectQueryIntegrationTests38 : ObjectQueryIntegrationTests38Base, IClassFixture<GremlinServer38ContainerFixture>
    {
        public ObjectQueryIntegrationTests38(GremlinServer38ContainerFixture fixture) : base(fixture, new ObjectQueryExecutingVerifier())
        {
        }
    }
}
