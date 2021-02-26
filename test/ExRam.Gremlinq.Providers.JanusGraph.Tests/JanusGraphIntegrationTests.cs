#if RELEASE && NET5_0 && RUNJANUSGRAPHINTEGRATIONTESTS
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class JanusGraphIntegrationTests : QueryIntegrationTest
    {
        public JanusGraphIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseJanusGraph(builder => builder
                        .AtLocalhost())
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default)),
            testOutputHelper)
        {
        }
    }
}
#endif
