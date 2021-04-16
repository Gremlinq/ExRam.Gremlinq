namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseJanusGraph(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource environment, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.WebSocket.IProviderConfigurator<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>
    {
        ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator At(System.Uri uri);
    }
}