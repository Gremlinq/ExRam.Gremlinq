namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNeptune(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator
    {
        ExRam.Gremlinq.Providers.Neptune.INeptuneConfiguratorWithUri At(System.Uri uri);
    }
    public interface INeptuneConfiguratorWithUri : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation
    {
        ExRam.Gremlinq.Providers.Neptune.INeptuneConfiguratorWithUri ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator> transformation);
    }
    public static class NeptuneConfiguratorWithUriExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfiguratorWithUri UseElasticSearch(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfiguratorWithUri configurator, System.Uri elasticSearchEndPoint) { }
    }
}