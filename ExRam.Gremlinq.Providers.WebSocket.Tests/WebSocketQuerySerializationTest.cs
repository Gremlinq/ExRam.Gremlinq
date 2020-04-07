using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class WebSocketQuerySerializationTest : QueryExecutionTest
    {
        public WebSocketQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseWebSocket(builder => builder
                        .AtLocalhost())
                    .UseExecutor(GremlinQueryExecutor.Echo)
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Identity)),
            testOutputHelper)
        {

        }
    }
}
