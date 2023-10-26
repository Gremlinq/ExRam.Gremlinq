namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseJanusGraph<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.IGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
    public interface IJanusGraphConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>, ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator> { }
}