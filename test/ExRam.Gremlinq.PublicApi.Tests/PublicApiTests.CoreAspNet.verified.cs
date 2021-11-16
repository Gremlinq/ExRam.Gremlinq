namespace ExRam.Gremlinq.Core.AspNet
{
    public readonly struct GremlinqSetup
    {
        public GremlinqSetup(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection) { }
        public Microsoft.Extensions.DependencyInjection.IServiceCollection ServiceCollection { get; }
    }
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup ConfigureEnvironment(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.IGremlinQueryEnvironment> environmentTransformation) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup ConfigureQuerySource(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.IGremlinQuerySource> sourceTranformation) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup RegisterTypes(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<Microsoft.Extensions.DependencyInjection.IServiceCollection> registration) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseConfigurationSection(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, string sectionName) { }
    }
    public interface IGremlinqConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.IConfigurationSection { }
    public interface IProviderConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.IConfigurationSection { }
    public static class ProviderSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.ProviderSetup<TConfigurator> Configure<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.ProviderSetup<TConfigurator> setup, System.Func<TConfigurator, ExRam.Gremlinq.Core.AspNet.IGremlinqConfigurationSection, ExRam.Gremlinq.Core.AspNet.IProviderConfigurationSection, TConfigurator> extraConfiguration)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
    }
    public readonly struct ProviderSetup<TConfigurator>
        where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        public ProviderSetup(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection) { }
        public Microsoft.Extensions.DependencyInjection.IServiceCollection ServiceCollection { get; }
    }
}
namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions { }
}
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions { }
}