namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseProvider<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, string sectionName, System.Func<ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection, ExRam.Gremlinq.Core.IGremlinQuerySource> providerChoice)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
}
namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IProviderConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.IConfigurationSection
    {
        ExRam.Gremlinq.Core.AspNet.IGremlinqConfigurationSection GremlinqSection { get; }
    }
    public static class ProviderConfiguratorExtensions
    {
        public static TConfigurator ConfigureBase<TConfigurator>(this TConfigurator configurator, ExRam.Gremlinq.Core.AspNet.IGremlinqConfigurationSection section)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
        public static TConfigurator ConfigureWebSocket<TConfigurator>(this TConfigurator configurator, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection section)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}