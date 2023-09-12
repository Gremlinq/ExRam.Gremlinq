using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class JanusGraphContainerFixture : TestContainerFixture
    {
        public JanusGraphContainerFixture() : base("janusgraph/janusgraph")
        {

        }

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IConfigurableGremlinQuerySource g) => g
            .UseJanusGraph(builder => builder
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()));
    }
}
