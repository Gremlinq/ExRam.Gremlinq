using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ProviderSetup<IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinqSetup setup)
        {
            return setup
                .UseProvider<IJanusGraphConfigurator>(
                    "JanusGraph",
                    (source, configurationContinuation) => source.UseJanusGraph<TVertexBase, TEdgeBase>(configurationContinuation))
                .Configure((configurator, section) => configurator
                    .ConfigureBase(section.GremlinqSection)
                    .ConfigureWebSocket(section));
        }
    }
}
