using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> without element projection against Gremlin Server 3.8.2. Owns no snapshots:
    /// <see cref="IntegrationWithoutProjectionTests38Base"/> redirects every fact.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class IntegrationWithoutProjectionTests38 : IntegrationWithoutProjectionTests38Base, IClassFixture<IntegrationWithoutProjectionTests38.Fixture>
    {
        public sealed class Fixture : GremlinServer38ContainerFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .WithoutElementProjection();
        }

        public IntegrationWithoutProjectionTests38(Fixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }
    }
}
