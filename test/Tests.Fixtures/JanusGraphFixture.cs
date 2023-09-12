using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class JanusGraphFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IConfigurableGremlinQuerySource g) => g
            .UseJanusGraph(builder => builder
                .AtLocalhost()
                .UseNewtonsoftJson())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()));
    }
}
