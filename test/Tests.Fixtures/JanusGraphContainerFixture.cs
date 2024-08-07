using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class JanusGraphContainerFixture : ImageTestContainerFixture
    {
        public JanusGraphContainerFixture() : base("janusgraph/janusgraph:1.0.0")
        {

        }

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseJanusGraph<Vertex, Edge>(builder => builder
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()))
            .IgnoreCosmosDbSpecificProperties();
    }
}
