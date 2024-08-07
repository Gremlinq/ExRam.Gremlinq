using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class JanusGraphFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
            .UseJanusGraph<Vertex, Edge>(builder => builder
                .AtLocalhost()
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()))
            .IgnoreCosmosDbSpecificProperties();
    }
}
