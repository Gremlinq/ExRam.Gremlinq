namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseJanusGraph<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, Microsoft.Extensions.Configuration.IConfigurationSection, ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>? configuratorTransformation = null) { }
    }
}