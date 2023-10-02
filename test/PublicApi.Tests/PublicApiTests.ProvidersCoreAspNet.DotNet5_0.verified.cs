namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqConfiguratorExtensions
    {
        public static TConfigurator ConfigureWebSocket<TConfigurator>(this TConfigurator configurator, Microsoft.Extensions.Configuration.IConfigurationSection section)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}