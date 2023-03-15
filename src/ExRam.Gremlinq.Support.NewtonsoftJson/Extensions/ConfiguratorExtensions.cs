namespace ExRam.Gremlinq.Core
{
    public static class ConfiguratorExtensions
    {
        public static TConfigurator UseNewtonsoftJson<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            return configurator.ConfigureDeserialization(transformer =>  transformer.UseNewtonsoftJson());
        }
    }
}
