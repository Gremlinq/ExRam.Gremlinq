using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class IntegrationWithoutProjectionTests : QueryExecutionTestsBase, IClassFixture<IntegrationWithoutProjectionTests.Fixture>, ISourceFileNameProvider<IntegrationWithoutProjectionTests>
    {
        public sealed class Fixture : GremlinServerContainerFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .WithoutElementProjection();
        }

        public IntegrationWithoutProjectionTests(Fixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
