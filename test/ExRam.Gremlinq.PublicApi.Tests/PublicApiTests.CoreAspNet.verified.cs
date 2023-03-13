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
}
namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetRequiredConfiguration(this Microsoft.Extensions.Configuration.IConfiguration configuration, string key) { }
    }
}
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddGremlinq(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection, System.Action<ExRam.Gremlinq.Core.AspNet.GremlinqSetup> configuration) { }
    }
}