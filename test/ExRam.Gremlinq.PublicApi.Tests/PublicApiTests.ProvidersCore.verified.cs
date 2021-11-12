namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfigurator<out TConfigurator> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation
        where out TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        TConfigurator At(System.Uri uri);
    }
}
namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class ProviderConfiguratorExtensions
    {
        public static TProviderConfigurator AtLocalhost<TProviderConfigurator>(this TProviderConfigurator configurator)
            where TProviderConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TProviderConfigurator> { }
    }
}