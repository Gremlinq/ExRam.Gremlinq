using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Support.TestContainers;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class JanusGraphContainerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseJanusGraph<Vertex, Edge>(builder => builder
                .UseJanusGraphContainer("1.1.0")
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()))
            .IgnoreCosmosDbSpecificProperties();
    }
}
