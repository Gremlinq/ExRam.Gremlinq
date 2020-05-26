[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
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
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseConfigurationSection(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, string sectionName) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseModel(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, ExRam.Gremlinq.Core.IGraphModel model) { }
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