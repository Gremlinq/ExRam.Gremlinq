using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class WebSocketGroovySerializationTest : GroovySerializationTest
    {
        public WebSocketGroovySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseWebSocket(builder => builder
                        .AtLocalhost()
                        .SetGraphSONVersion(GraphsonVersion.V2))),
            testOutputHelper)
        {

        }
    }
}
