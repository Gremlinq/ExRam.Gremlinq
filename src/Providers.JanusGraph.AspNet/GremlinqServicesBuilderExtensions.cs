using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqProviderServicesBuilder<IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
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
