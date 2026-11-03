using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> against Gremlin Server 3.8.2. Owns the snapshots of the facts that answer
    /// differently than on 3.7; <see cref="IntegrationTests38Base"/> names them.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class IntegrationTests38 : IntegrationTests38Base, IClassFixture<GremlinServer38ContainerFixture>, ISourceFileNameProvider<IntegrationTests38>
    {
        public IntegrationTests38(GremlinServer38ContainerFixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
