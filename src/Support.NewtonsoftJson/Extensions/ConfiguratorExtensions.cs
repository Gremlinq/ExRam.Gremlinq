using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    /// <summary>Extension methods for configuring Newtonsoft.Json support on provider configurators.</summary>
    public static class ConfiguratorExtensions
    {
        /// <summary>Configures the provider to use Newtonsoft.Json for serialization.</summary>
        /// <typeparam name="TConfigurator">The configurator type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        public static TConfigurator UseNewtonsoftJson<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : IGremlinqConfigurator<TConfigurator> => configurator
                .ConfigureQuerySource(source => source
                    .ConfigureEnvironment(environment => environment
                        .UseNewtonsoftJson()));
    }
}
