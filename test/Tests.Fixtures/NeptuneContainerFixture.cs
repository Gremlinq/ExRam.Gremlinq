using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Support.NewtonsoftJson;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class NeptuneContainerFixture : ImageTestContainerFixture
    {
        public NeptuneContainerFixture() : base("tinkerpop/gremlin-server:3.7.2")
        {
        }

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson())
            .IgnoreCosmosDbSpecificProperties();
    }
}
