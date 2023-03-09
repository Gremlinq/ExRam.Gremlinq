using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public sealed class WebSocketQuerySerializationTest : QuerySerializationTest, IClassFixture<WebSocketQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseWebSocket(builder => builder
                    .AtLocalhost())
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Identity)
                    .UseNewtonsoftJson()))
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
