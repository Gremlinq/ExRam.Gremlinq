using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IGremlinqServicesBuilder
    {
        IGremlinqServicesBuilder FromBaseSection(string sectionName);
        
        IGremlinqServicesBuilder ConfigureQuerySource(Func<IGremlinQuerySource, IConfigurationSection, IGremlinQuerySource> sourceTranformation);

        IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
            where TTransformation : class, IGremlinQuerySourceTransformation;

        IServiceCollection Services { get; }
    }

    public interface IGremlinqServicesBuilder<TConfigurator> : IGremlinqServicesBuilder
     where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        IGremlinqServicesBuilder<TConfigurator> FromProviderSection(string sectionName);

        IGremlinqServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration);

        IGremlinqServicesBuilder<TConfigurator> Configure<TConfiguratorTransformation>()
            where TConfiguratorTransformation : class, IGremlinqConfiguratorTransformation<TConfigurator>;
    }
}
