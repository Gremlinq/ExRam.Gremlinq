using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class IntegrationWithoutProjectionTests : QueryExecutionTest, IClassFixture<IntegrationWithoutProjectionTests.Fixture>, ISourceFileNameProvider<IntegrationWithoutProjectionTests>
    {
        public class Fixture : GremlinServerWithoutProjectionContainerFixtureBase
        {
            public Fixture() : base(TinkerPopVersions.V3_7_7)
            {
            }
        }

        public IntegrationWithoutProjectionTests(Fixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
