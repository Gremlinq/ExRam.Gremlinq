namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderSetup<TConfigurator> UseProvider<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqSetup setup, string sectionName, System.Func<ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, System.Func<TConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation>, ExRam.Gremlinq.Core.IGremlinQuerySource> providerChoice)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
    public static class ProviderSetupExtensions { }
}
namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderSetup<TConfigurator> : ExRam.Gremlinq.Core.AspNet.IGremlinqSetup
        where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderSetup<TConfigurator> Configure(System.Func<TConfigurator, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection, TConfigurator> extraConfiguration);
        ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderSetup<TConfigurator> Configure<TProviderConfiguratorTransformation>()
            where TProviderConfiguratorTransformation :  class, ExRam.Gremlinq.Providers.Core.IProviderConfiguratorTransformation<TConfigurator>;
    }
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