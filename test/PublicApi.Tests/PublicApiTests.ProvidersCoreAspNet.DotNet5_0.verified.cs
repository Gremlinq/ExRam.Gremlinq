namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqProviderServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> FromSection<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder, string sectionName)
            where TConfigurator : ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator> { }
    }
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> UseProvider<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup, System.Func<ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, System.Func<System.Func<TConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation>, ExRam.Gremlinq.Core.IGremlinQuerySource>> providerChoice)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
}
namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IProviderConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.IConfigurationSection { }
    public static class ProviderConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> ConfigureBase<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> ConfigureWebSocket<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}