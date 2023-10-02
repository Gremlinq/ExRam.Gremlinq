using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Providers.JanusGraph.AspNet.Extensions;

namespace ExRam.Gremlinq.Providers.JanusGraph.AspNet.Extensions
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
                        .ConfigureWebSocket(providerSection);
                });
        }
    }
}
