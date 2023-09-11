using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class JanusGraphContainerFixture : TestContainerFixture
    {
        public JanusGraphContainerFixture() : base(
            "janusgraph/janusgraph",
            8182,
            container => g
                .UseJanusGraph(builder => builder
                    .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                    .UseNewtonsoftJson())
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .IgnoreResults())))
        {
        }
    }
}
