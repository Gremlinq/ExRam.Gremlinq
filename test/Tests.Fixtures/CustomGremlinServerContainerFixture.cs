using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class CustomGremlinServerContainerFixture : DockerfileTestContainerFixture
    {
        private readonly Version _version;

        public CustomGremlinServerContainerFixture() : this(TinkerPopVersions.V3_7_7)
        {
        }

        protected CustomGremlinServerContainerFixture(Version version) : base("CustomGremlinServerDockerfile", buildArguments: new Dictionary<string, string> { ["GREMLIN_SERVER_VERSION"] = version.ToString() })
        {
            _version = version;
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson()
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(_version, _version)))
            .IgnoreCosmosDbSpecificProperties();
    }
}
