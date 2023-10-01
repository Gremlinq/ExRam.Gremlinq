using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderServicesBuilder<TConfigurator> : IGremlinqServicesBuilder
         where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        IGremlinqProviderServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration);

        IGremlinqProviderServicesBuilder<TConfigurator> Configure<TProviderConfiguratorTransformation>()
            where TProviderConfiguratorTransformation : class, IProviderConfiguratorTransformation<TConfigurator>;
    }
}
