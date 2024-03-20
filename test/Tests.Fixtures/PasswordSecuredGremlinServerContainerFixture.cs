using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class PasswordSecuredGremlinServerContainerFixture : DockerfileTestContainerFixture
    {
        public PasswordSecuredGremlinServerContainerFixture() : base("PasswordSecureGremlinServerDockerfile")
        {
        }


        protected override async Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .ConfigureClientFactory(factory => factory
                    .ConfigureBaseFactory(factory => factory
                        .WithPlainCredentials("stephen", "password")))
                .UseNewtonsoftJson());
    }
}
