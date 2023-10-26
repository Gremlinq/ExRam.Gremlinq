using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    public static class ConfiguratorExtensions
    {
        public static TConfigurator UseNewtonsoftJson<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            return configurator
                .ConfigureQuerySource(source => source
                    .ConfigureEnvironment(environment => environment
                        .UseNewtonsoftJson()));
        }
    }
}
