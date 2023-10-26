using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class JanusGraphContainerFixture : TestContainerFixture
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
                    .IgnoreResults()));
    }
}
