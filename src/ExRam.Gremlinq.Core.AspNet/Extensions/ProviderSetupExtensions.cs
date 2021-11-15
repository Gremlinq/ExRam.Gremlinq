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
            private readonly IProviderConfigurationSection _configuration;
            private readonly Func<TConfigurator, IProviderConfigurationSection, TConfigurator> _extraConfiguration;

            public ExtraConfigurationProviderConfiguratorTransformation(IProviderConfigurationSection configuration, Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration)
            {
                _configuration = configuration;
                _extraConfiguration = extraConfiguration;
            }

            public TConfigurator Transform(TConfigurator configurator) => _extraConfiguration(configurator, _configuration);
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
