using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .ConfigureBase()
                .UseProvider<IJanusGraphConfigurator>(source => source
                    .UseJanusGraph<TVertexBase, TEdgeBase>)
                .ConfigureWebSocket();
        }
    }
}
