namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqProviderServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> FromSection<TConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> builder, string sectionName)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> UseProvider<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup, System.Func<ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, System.Func<TConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation>, ExRam.Gremlinq.Core.IGremlinQuerySource> providerChoice)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
}
namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderServicesBuilder<TConfigurator> : ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder
        where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> Configure(System.Func<TConfigurator, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection, TConfigurator> extraConfiguration);
        ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> Configure<TProviderConfiguratorTransformation>()
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