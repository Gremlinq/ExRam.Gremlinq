using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerIntegrationTests : QueryIntegrationTest, IClassFixture<GremlinServerIntegrationTests.Fixture>
    {
        public new sealed class Fixture : QueryIntegrationTest.Fixture
        {
            public Fixture() : base(g
                .UseGremlinServer(builder => builder
                    .AtLocalhost()
                    .UseNewtonsoftJson()))
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
