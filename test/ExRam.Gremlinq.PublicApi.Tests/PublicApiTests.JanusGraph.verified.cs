namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseJanusGraph(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurator
    {
        ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfiguratorWithUri At(System.Uri uri);
    }
    public interface IJanusGraphConfiguratorWithUri : ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator> transformation);
    }
}