#if RELEASE && NETCOREAPP3_1
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class JanusGraphIntegrationTests : QueryExecutionTest
    {
        public JanusGraphIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseJanusGraph(builder => builder
                        .AtLocalhost())),
            testOutputHelper)
        {
        }
    }
}
#endif
