using ExRam.Gremlinq.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class IntegrationTests : Providers.Tests.IntegrationTests
    {
        public IntegrationTests() : base(g
            .WithWebSocket("localhost", GraphsonVersion.V3))
        {
        }
    }
}
