using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class IntegrationTests : Providers.Tests.IntegrationTests
    {
        public IntegrationTests() : base(g.ConfigureEnvironment(env => env
            .UseWebSocket("localhost", GraphsonVersion.V3)))
        {
        }
    }
}
