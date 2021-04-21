namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseJanusGraph(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource environment, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>, ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator> { }
}