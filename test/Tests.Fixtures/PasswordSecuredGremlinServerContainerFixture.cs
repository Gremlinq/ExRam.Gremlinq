using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// A Gremlin Server 3.7.7 that asks for a password, from <c>PasswordSecureGremlinServerDockerfile</c>, and a client that
    /// gives the right one. A fixture for another password calls the protected constructor.
    /// </summary>
    public class PasswordSecuredGremlinServerContainerFixture : DockerfileTestContainerFixture
    {
        private readonly string _password;

        public PasswordSecuredGremlinServerContainerFixture() : this("password")
        {
        }

        protected PasswordSecuredGremlinServerContainerFixture(string password) : base("PasswordSecureGremlinServerDockerfile")
        {
            _password = password;
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .ConfigureClientFactory(factory => factory
                    .ConfigureBaseFactory(factory => factory
                        .WithPlainCredentials("stephen", _password)))
                .UseNewtonsoftJson())
            .IgnoreCosmosDbSpecificProperties();
    }
}
