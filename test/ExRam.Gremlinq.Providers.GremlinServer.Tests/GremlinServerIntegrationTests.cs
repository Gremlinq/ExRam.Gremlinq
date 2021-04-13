#if RELEASE && NET5_0 && RUNGREMLINSERVERINTEGRATIONTESTS
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerIntegrationTests : QueryIntegrationTest, IClassFixture<GremlinServerFixture>
    {
        public GremlinServerIntegrationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
#endif
