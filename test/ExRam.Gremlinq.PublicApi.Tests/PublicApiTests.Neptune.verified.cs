namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNeptune(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.WebSocket.IProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>
    {
        ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator At(System.Uri uri);
    }
    public static class NeptuneConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator AtLocalhost(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator configurator) { }
    }
    public static class NeptuneConfiguratorWithUriExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator UseElasticSearch(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator configurator, System.Uri elasticSearchEndPoint) { }
    }
}