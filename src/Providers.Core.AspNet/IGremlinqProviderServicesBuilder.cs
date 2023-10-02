using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderServicesBuilder<TConfigurator> : IGremlinqServicesBuilder
         where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        IGremlinqProviderServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration);

        IGremlinqProviderServicesBuilder<TConfigurator> Configure<TProviderConfiguratorTransformation>()
            where TProviderConfiguratorTransformation : class, IGremlinqConfiguratorTransformation<TConfigurator>;
    }
}
