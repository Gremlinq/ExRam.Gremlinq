using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GremlinServerContainerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .UseGremlinServerContainer("3.7.5")
                .ConfigureClientFactory(factory => factory
                    .ConfigureClient(client => client
                        .TransformRequest(async (requestMessage, _) => requestMessage  //Just for demo/coverage purposes.
                        /*.Rebuild()
                        /*.Create()*/)))
                .UseNewtonsoftJson())
            .IgnoreCosmosDbSpecificProperties();
    }
}
