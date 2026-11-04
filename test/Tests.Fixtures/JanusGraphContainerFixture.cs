using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Support.TestContainers;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// JanusGraph 1.1.0 in a container, read through Newtonsoft.Json. The executor ignores the results: JanusGraph answers
    /// with ids of its own, so the facts only prove that their queries execute.
    /// </summary>
    public class JanusGraphContainerFixture : GremlinqFixture
    {
        protected virtual IJanusGraphConfigurator ConfigureProvider(IJanusGraphConfigurator configurator) => configurator
            .UseJanusGraphContainer("1.1.0");

        protected virtual IJanusGraphConfigurator AddJsonSupport(IJanusGraphConfigurator configurator) => configurator
            .UseNewtonsoftJson();

        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseJanusGraph<Vertex, Edge>(configurator => AddJsonSupport(ConfigureProvider(configurator)))
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults()))
            .IgnoreCosmosDbSpecificProperties();
    }
}
