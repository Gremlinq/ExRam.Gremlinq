using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class SimpleJanusGraphFixture : GremlinqFixture
    {
        public SimpleJanusGraphFixture() : base(g
            .UseJanusGraph(builder => builder
                .AtLocalhost()
                .UseNewtonsoftJson()))
        {
        }
    }
}
