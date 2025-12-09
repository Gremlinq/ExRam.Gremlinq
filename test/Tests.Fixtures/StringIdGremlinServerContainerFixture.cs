using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class StringIdGremlinServerContainerFixture : DockerfileTestContainerFixture
    {
        public StringIdGremlinServerContainerFixture() : base("StringIdGremlinServerDockerfile")
        {
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson())
                        .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()))
            .IgnoreCosmosDbSpecificProperties();
    }
}
