namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfiguratorTransformation<TConfigurator>
        where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        TConfigurator Transform(TConfigurator configurator);
    }
    public interface IProviderConfigurator<out TConfigurator> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator>
        where out TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator> { }
}