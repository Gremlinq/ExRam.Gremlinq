using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneContainerFixture : TestContainerFixture
    {
        public NeptuneContainerFixture() : base("tinkerpop/gremlin-server:3.7.0")
        {
        }

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IConfigurableGremlinQuerySource g) => g
            .UseNeptune(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson());
    }
}
