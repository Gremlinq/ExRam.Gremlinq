using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IGremlinqServicesBuilder
    {
        IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
            where TTransformation : class, IGremlinQuerySourceTransformation;

        IGremlinqServicesBuilder UseConfigurationSection(string sectionName);
        
        IGremlinqServicesBuilder ConfigureQuerySource(Func<IGremlinQuerySource, IConfigurationSection, IGremlinQuerySource> sourceTranformation);

        IServiceCollection Services { get; }
    }

    public interface IGremlinqServicesBuilder<TConfigurator> : IGremlinqServicesBuilder
     where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        IGremlinqServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration);

        IGremlinqServicesBuilder<TConfigurator> Configure<TConfiguratorTransformation>()
            where TConfiguratorTransformation : class, IGremlinqConfiguratorTransformation<TConfigurator>;
    }
}
