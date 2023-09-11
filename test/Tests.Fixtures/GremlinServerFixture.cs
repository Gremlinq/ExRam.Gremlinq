using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GremlinServerFixture : TestContainerFixture
    {
        public GremlinServerFixture() : base(
            "tinkerpop/gremlin-server:3.7.0",
            8182,
            container => g
                .UseGremlinServer(_ => _
                    .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                    .UseNewtonsoftJson()))
        {
        }
    }
}
