using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.NewtonsoftJson;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class WrongPasswordGremlinServerContainerFixture : DockerfileTestContainerFixture
    {
        public WrongPasswordGremlinServerContainerFixture() : base("Dockerfiles/PasswordSecureGremlinServerDockerfile")
        {
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .ConfigureClientFactory(factory => factory
                    .ConfigureBaseFactory(factory => factory
                        .WithPlainCredentials("stephen", "wrongPassword")))
                .UseNewtonsoftJson());
    }
}
