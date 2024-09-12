using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GremlinServerWithoutProjectionContainerFixture : ImageTestContainerFixture
    {
        public GremlinServerWithoutProjectionContainerFixture(IMessageSink messageSink) : base("tinkerpop/gremlin-server:3.7.2", 8182, messageSink)
        {
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.VertexProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.EdgeProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.VertexPropertyProjectionSteps, Traversal.Empty)))
            .IgnoreCosmosDbSpecificProperties();
    }
}
