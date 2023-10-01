namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IGremlinqConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.IConfigurationSection { }
    public interface IGremlinqServicesBuilder
    {
        Microsoft.Extensions.DependencyInjection.IServiceCollection Services { get; }
        ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder ConfigureQuerySource(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.IGremlinQuerySource> sourceTranformation);
        ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
            where TTransformation :  class, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation;
        ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder UseConfigurationSection(string sectionName);
    }
}
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddGremlinq(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection, System.Action<ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder> configuration) { }
    }
}