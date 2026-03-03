using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    /// <summary>Builder for configuring Gremlinq services in the dependency injection container.</summary>
    public interface IGremlinqServicesBuilder
    {
        /// <summary>Configures the base configuration section name.</summary>
        /// <param name="sectionName">The section name.</param>
        IGremlinqServicesBuilder FromBaseSection(string sectionName);

        /// <summary>Configures the query source using a configuration section.</summary>
        /// <param name="sourceTranformation">The transformation function.</param>
        IGremlinqServicesBuilder ConfigureQuerySource(Func<IGremlinQuerySource, IConfigurationSection, IGremlinQuerySource> sourceTranformation);

        /// <summary>Configures the query source using a registered transformation type.</summary>
        /// <typeparam name="TTransformation">The transformation type.</typeparam>
        IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
            where TTransformation : class, IGremlinQuerySourceTransformation;

        /// <summary>Gets the service collection.</summary>
        IServiceCollection Services { get; }
    }

    /// <summary>Builder for configuring Gremlinq services with a specific provider configurator.</summary>
    /// <typeparam name="TConfigurator">The provider configurator type.</typeparam>
    public interface IGremlinqServicesBuilder<TConfigurator> : IGremlinqServicesBuilder
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        /// <summary>Configures the provider using a configuration section.</summary>
        /// <param name="extraConfiguration">The configuration function.</param>
        IGremlinqServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration);

        /// <summary>Configures the provider using a registered transformation type.</summary>
        /// <typeparam name="TConfiguratorTransformation">The transformation type.</typeparam>
        IGremlinqServicesBuilder<TConfigurator> Configure<TConfiguratorTransformation>()
            where TConfiguratorTransformation : class, IGremlinqConfiguratorTransformation<TConfigurator>;
    }
}
