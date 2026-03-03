using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Support.TestContainers;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class StringIdGremlinServerContainerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .UseGremlinServerModContainer()
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()))
            .IgnoreCosmosDbSpecificProperties();
    }
}
