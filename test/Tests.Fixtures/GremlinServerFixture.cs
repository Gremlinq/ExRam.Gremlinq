using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GremlinServerFixture : GremlinqFixture
    {
        public GremlinServerFixture() : base(g
            .UseGremlinServer(_ => _
                .At(new Uri("ws://gremlinServer:8182"))
                .UseNewtonsoftJson()))
        {
        }
    }
}
