using System;
using ExRam.Gremlinq.Core.AspNet;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public static class ProviderSetupExtensions
    {
        private sealed class ExtraConfigurationProviderConfiguratorTransformation<TConfigurator> : IProviderConfiguratorTransformation<TConfigurator>
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private readonly IGremlinqConfigurationSection _gremlinqSection;
            private readonly IProviderConfigurationSection _providerSection;
            private readonly Func<TConfigurator, IGremlinqConfigurationSection, IProviderConfigurationSection, TConfigurator> _extraConfiguration;

            public ExtraConfigurationProviderConfiguratorTransformation(IGremlinqConfigurationSection gremlinqSection, IProviderConfigurationSection providerSection, Func<TConfigurator, IGremlinqConfigurationSection, IProviderConfigurationSection, TConfigurator> extraConfiguration)
            {
                _gremlinqSection = gremlinqSection;
                _providerSection = providerSection;
                _extraConfiguration = extraConfiguration;
            }

            public TConfigurator Transform(TConfigurator configurator) => _extraConfiguration(configurator, _gremlinqSection, _providerSection);
        }

        public static ProviderSetup<TConfigurator> Configure<TConfigurator>(this ProviderSetup<TConfigurator> setup, Func<TConfigurator, IGremlinqConfigurationSection, IProviderConfigurationSection, TConfigurator> extraConfiguration)
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return new ProviderSetup<TConfigurator>(setup
                .ServiceCollection
                .AddSingleton<IProviderConfiguratorTransformation<TConfigurator>>(s => new ExtraConfigurationProviderConfiguratorTransformation<TConfigurator>(
                    s.GetRequiredService<IGremlinqConfigurationSection>(),
                    s.GetRequiredService<IProviderConfigurationSection>(),
                    extraConfiguration)));
        }
    }
}
