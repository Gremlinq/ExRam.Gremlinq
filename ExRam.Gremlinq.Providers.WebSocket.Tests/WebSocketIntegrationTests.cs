using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class WebSocketIntegrationTests : IntegrationTests
    {
        public WebSocketIntegrationTests() : base(g.ConfigureEnvironment(env => env.UseWebSocket(builder => { })))
        {
        }
    }
}
