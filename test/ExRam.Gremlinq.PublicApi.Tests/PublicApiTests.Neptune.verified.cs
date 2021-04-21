namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNeptune(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>, ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator> { }
    public static class NeptuneConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator UseElasticSearch(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator configurator, System.Uri elasticSearchEndPoint, ExRam.Gremlinq.Providers.Neptune.NeptuneElasticSearchIndexConfiguration indexConfiguration = 0) { }
    }
    public enum NeptuneElasticSearchIndexConfiguration
    {
        Standard = 0,
        LowercaseKeyword = 1,
    }
}