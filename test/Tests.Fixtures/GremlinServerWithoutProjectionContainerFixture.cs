using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GremlinServerWithoutProjectionContainerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .UseGremlinServerContainer("3.7.6")
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.VertexProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.EdgeProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.VertexPropertyProjectionSteps, Traversal.Empty)))
            .IgnoreCosmosDbSpecificProperties();
    }
}
