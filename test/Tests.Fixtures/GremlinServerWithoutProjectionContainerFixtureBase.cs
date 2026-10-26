using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class GremlinServerWithoutProjectionContainerFixtureBase : GremlinqFixture
    {
        private readonly Version _version;

        protected GremlinServerWithoutProjectionContainerFixtureBase(Version version)
        {
            _version = version;
        }

        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .UseGremlinServerContainer(_version.ToString())
                .UseNewtonsoftJson()
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(_version, _version)))
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.VertexProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.EdgeProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.VertexPropertyProjectionSteps, Traversal.Empty)))
            .IgnoreCosmosDbSpecificProperties();
    }
}
