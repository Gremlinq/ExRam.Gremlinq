#if RELEASE && NET5_0 && RUNGREMLINSERVERINTEGRATIONTESTS
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerIntegrationTests : QueryIntegrationTest, IClassFixture<GremlinServerIntegrationTests.Fixture>
    {
        public new sealed class Fixture : QueryIntegrationTest.Fixture
        {
            public Fixture() : base(Core.GremlinQuerySource.g
                .UseGremlinServer(builder => builder
                    .AtLocalhost()))
            {
            }
        }

        public GremlinServerIntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
#endif
