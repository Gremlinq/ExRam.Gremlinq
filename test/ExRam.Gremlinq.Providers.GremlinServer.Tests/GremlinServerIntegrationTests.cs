#if RELEASE && NET5_0 && RUNGREMLINSERVERINTEGRATIONTESTS
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerIntegrationTests : QueryIntegrationTest
    {
        public GremlinServerIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g
                .UseGremlinServer(builder => builder
                    .AtLocalhost()),
            testOutputHelper)
        {
        }
    }
}
#endif
