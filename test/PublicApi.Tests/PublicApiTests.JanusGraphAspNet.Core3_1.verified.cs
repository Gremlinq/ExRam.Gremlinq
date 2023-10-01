namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderSetup<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.IGremlinqSetup setup) { }
    }
}