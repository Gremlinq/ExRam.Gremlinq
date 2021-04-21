using System;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class ProviderConfiguratorExtensions
    {
        public static TProviderConfigurator AtLocalhost<TProviderConfigurator>(this TProviderConfigurator configurator)
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            return configurator.At(new Uri("ws://localhost:8182"));
        }
    }
}
