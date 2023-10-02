using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IGremlinqProviderServicesBuilder<TConfigurator> : IGremlinqServicesBuilder
         where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        IGremlinqProviderServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration);

        IGremlinqProviderServicesBuilder<TConfigurator> Configure<TConfiguratorTransformation>()
            where TConfiguratorTransformation : class, IGremlinqConfiguratorTransformation<TConfigurator>;
    }
}
