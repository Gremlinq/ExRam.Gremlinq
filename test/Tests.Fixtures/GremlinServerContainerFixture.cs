using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GremlinServerContainerFixture : ImageTestContainerFixture
    {
        public GremlinServerContainerFixture() : base("tinkerpop/gremlin-server:3.7.4")
        {
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .ConfigureClientFactory(factory => factory
                    .ConfigureClient(client => client
                        .TransformRequest(async (requestMessage, ct) => requestMessage  //Just for demo/coverage purposes.
                        /*.Rebuild()
                        /*.Create()*/)))
                .UseNewtonsoftJson())
            .IgnoreCosmosDbSpecificProperties();
    }
}
