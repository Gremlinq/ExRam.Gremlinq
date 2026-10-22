using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GremlinServer38ContainerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .UseGremlinServerContainer("3.8.2")
                .UseNewtonsoftJson()
                .WithCompatibility(VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)))
            .IgnoreCosmosDbSpecificProperties();
    }
}
