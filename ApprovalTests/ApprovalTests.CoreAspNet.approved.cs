[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core.AspNet
{
    public readonly struct GremlinqOptions
    {
        public GremlinqOptions(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection) { }
        public Microsoft.Extensions.DependencyInjection.IServiceCollection ServiceCollection { get; }
    }
    public static class GremlinqOptionsExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqOptions UseConfigurationSection(this ExRam.Gremlinq.Core.AspNet.GremlinqOptions options, string sectionName) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqOptions UseModel(this ExRam.Gremlinq.Core.AspNet.GremlinqOptions options, ExRam.Gremlinq.Core.IGraphModel model) { }
    }
    public interface IGremlinQueryEnvironmentTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironment Transform(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IGremlinqConfiguration : Microsoft.Extensions.Configuration.IConfiguration { }
}
namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions { }
}
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions { }
}