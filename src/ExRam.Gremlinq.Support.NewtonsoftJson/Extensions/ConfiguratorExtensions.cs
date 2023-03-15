namespace ExRam.Gremlinq.Core
{
    public static class ConfiguratorExtensions
    {
        public static TConfigurator UseNewtonsoftJson<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            return configurator
                .ConfigureQuerySource(source => source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureDeserializer(transformer =>  transformer
                            .UseNewtonsoftJson())));
        }
    }
}
