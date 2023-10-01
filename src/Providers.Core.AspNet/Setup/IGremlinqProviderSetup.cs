using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderSetup<TConfigurator> : IGremlinqSetup
         where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        IGremlinqProviderSetup<TConfigurator> Configure(Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration);

        IGremlinqProviderSetup<TConfigurator> Configure<TProviderConfiguratorTransformation>()
            where TProviderConfiguratorTransformation : class, IProviderConfiguratorTransformation<TConfigurator>;
    }
}
