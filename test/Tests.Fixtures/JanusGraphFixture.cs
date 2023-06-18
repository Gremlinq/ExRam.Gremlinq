using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class JanusGraphFixture : GremlinqFixture
    {
        public JanusGraphFixture() : base(Gremlinq.Core.GremlinQuerySource.g
            .UseJanusGraph(builder => builder
                .At(new Uri("ws://localhost:8183"))
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults())))
        {
        }
    }
}
