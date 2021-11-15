using System;
using ExRam.Gremlinq.Providers.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class ProviderSetupExtensions
    {
        private sealed class ExtraConfigurationProviderConfiguratorTransformation<TConfigurator> : IProviderConfiguratorTransformation<TConfigurator>
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private readonly IProviderConfigurationSection _providerSection;
            private readonly Func<TConfigurator, IProviderConfigurationSection, TConfigurator> _extraConfiguration;

            public ExtraConfigurationProviderConfiguratorTransformation(IProviderConfigurationSection providerSection, Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration)
            {
                _providerSection = providerSection;
                _extraConfiguration = extraConfiguration;
            }

            public TConfigurator Transform(TConfigurator configurator) => _extraConfiguration(configurator, _providerSection);
        }

        public static ProviderSetup<TConfigurator> Configure<TConfigurator>(this ProviderSetup<TConfigurator> setup, Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration)
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return new ProviderSetup<TConfigurator>(setup
                .ServiceCollection
                .AddSingleton<IProviderConfiguratorTransformation<TConfigurator>>(s => new ExtraConfigurationProviderConfiguratorTransformation<TConfigurator>(
                    s.GetRequiredService<IProviderConfigurationSection>(),
                    extraConfiguration)));
        }
    }
}
