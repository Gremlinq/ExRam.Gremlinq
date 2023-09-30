namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TConfigurator> UseProvider<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, string sectionName, System.Func<ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource, System.Func<TConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation>, ExRam.Gremlinq.Core.IGremlinQuerySource> providerChoice)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
    public static class ProviderSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TProviderConfigurator> Configure<TProviderConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TProviderConfigurator> setup, System.Func<TProviderConfigurator, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection, TProviderConfigurator> extraConfiguration)
            where TProviderConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TProviderConfigurator> { }
        public static ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TProviderConfigurator> Configure<TProviderConfigurator, TProviderConfiguratorTransformation>(this ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TProviderConfigurator> setup)
            where TProviderConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TProviderConfigurator>
            where TProviderConfiguratorTransformation :  class, ExRam.Gremlinq.Providers.Core.IProviderConfiguratorTransformation<TProviderConfigurator> { }
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
    public readonly struct ProviderSetup<TConfigurator>
        where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        public ProviderSetup(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection) { }
        public Microsoft.Extensions.DependencyInjection.IServiceCollection ServiceCollection { get; }
    }
}