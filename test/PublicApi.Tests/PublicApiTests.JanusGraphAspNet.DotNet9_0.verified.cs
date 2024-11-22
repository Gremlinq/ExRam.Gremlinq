namespace ExRam.Gremlinq.Providers.JanusGraph.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup) { }
    }
}