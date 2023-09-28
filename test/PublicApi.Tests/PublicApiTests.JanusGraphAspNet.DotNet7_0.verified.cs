namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseJanusGraph<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection, ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>? configuratorTransformation = null) { }
    }
}