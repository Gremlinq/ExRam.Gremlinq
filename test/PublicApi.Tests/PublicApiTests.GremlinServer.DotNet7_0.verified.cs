namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseGremlinServer<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.IGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
    public interface IGremlinServerConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator>, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
}