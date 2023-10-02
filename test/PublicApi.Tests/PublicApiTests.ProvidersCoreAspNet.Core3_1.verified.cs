namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqProviderServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> FromSection<TConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> builder, string sectionName)
            where TConfigurator : ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator> { }
    }
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> UseProvider<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup, System.Func<ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, System.Func<System.Func<TConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation>, ExRam.Gremlinq.Core.IGremlinQuerySource>> providerChoice)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
}
namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderServicesBuilder<TConfigurator> : ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder
        where TConfigurator : ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator>
    {
        ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> Configure(System.Func<TConfigurator, Microsoft.Extensions.Configuration.IConfigurationSection, TConfigurator> extraConfiguration);
        ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> Configure<TConfiguratorTransformation>()
            where TConfiguratorTransformation :  class, ExRam.Gremlinq.Core.IGremlinqConfiguratorTransformation<TConfigurator>;
    }
    public interface IProviderConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.IConfigurationSection { }
    public static class ProviderConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> ConfigureBase<TConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> ConfigureWebSocket<TConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}