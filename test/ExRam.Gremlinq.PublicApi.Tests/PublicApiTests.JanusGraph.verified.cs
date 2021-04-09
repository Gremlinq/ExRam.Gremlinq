namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseJanusGraph(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurationBuilder, ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurationBuilder
    {
        ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurationBuilderWithUri At(System.Uri uri);
    }
    public interface IJanusGraphConfigurationBuilderWithUri : ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}