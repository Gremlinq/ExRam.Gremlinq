using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public sealed class WebSocketQuerySerializationTest : QuerySerializationTest, IClassFixture<WebSocketQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : QuerySerializationTest.Fixture
        {
            public Fixture() : base(g.UseWebSocket(builder => builder
                .AtLocalhost()))
            {
            }
        }

        public WebSocketQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
