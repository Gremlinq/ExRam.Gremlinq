using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerFixture : IIntegrationTestFixture
    {
        public GremlinServerFixture()
        {
            GremlinQuerySource = Core.GremlinQuerySource.g
                .UseGremlinServer(builder => builder
                    .AtLocalhost());
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
