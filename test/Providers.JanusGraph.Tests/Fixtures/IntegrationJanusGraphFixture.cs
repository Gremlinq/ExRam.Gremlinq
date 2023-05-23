using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class IntegrationJanusGraphFixture : GremlinqFixture
    {
        public IntegrationJanusGraphFixture() : base(Gremlinq.Core.GremlinQuerySource.g
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
