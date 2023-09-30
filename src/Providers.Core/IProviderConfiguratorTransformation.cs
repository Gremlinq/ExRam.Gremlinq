namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfiguratorTransformation<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        TConfigurator Transform(TConfigurator configurator);
    }
}
