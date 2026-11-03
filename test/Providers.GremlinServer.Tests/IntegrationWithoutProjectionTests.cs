using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> without element projection against Gremlin Server 3.7.7. Owns the snapshots.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class IntegrationWithoutProjectionTests : IntegrationWithoutProjectionTestsBase, IClassFixture<IntegrationWithoutProjectionTests.Fixture>, ISourceFileNameProvider<IntegrationWithoutProjectionTests>
    {
        public sealed class Fixture : GremlinServerContainerFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .WithoutElementProjection();
        }

        public IntegrationWithoutProjectionTests(Fixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
