using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class NeptuneEngine15ContainerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(_ => _
                .UseIAMAuthentication(_ => _
                    .UseSigV4()
                    .WithUri(new Uri("http://localhost:8182"))
                    .WithAccessKeyId("accessKeyId")
                    .WithSecretAccessKey("secretAccessKey"))
                .UseGremlinServerContainer("3.8.2")
                .UseNewtonsoftJson()
                .WithEngineVersion(NeptuneEngineVersions.V1_5_0_0))
            .IgnoreCosmosDbSpecificProperties();
    }
}
