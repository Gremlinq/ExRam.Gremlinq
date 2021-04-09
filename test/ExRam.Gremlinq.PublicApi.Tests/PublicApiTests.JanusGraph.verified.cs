namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource UseJanusGraph(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource environment, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurator
    {
        ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfiguratorWithUri At(System.Uri uri);
    }
    public interface IJanusGraphConfiguratorWithUri : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator> transformation);
    }
}