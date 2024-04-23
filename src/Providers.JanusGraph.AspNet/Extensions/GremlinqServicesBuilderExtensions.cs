using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.JanusGraph.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .ConfigureBase()
                .UseProvider<IJanusGraphConfigurator>(source => source
                    .UseJanusGraph<TVertexBase, TEdgeBase>)
                .Configure((configurator, section) =>
                {
                    var providerSection = section
                        .GetSection("JanusGraph");

                    return configurator
                        .ConfigureWebSocket(providerSection)
                        .ConfigureBasicAuthentication(providerSection);
                });
        }
    }
}
