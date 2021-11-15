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
            private readonly IProviderConfiguration _configuration;
            private readonly Func<TConfigurator, IProviderConfiguration, TConfigurator> _extraConfiguration;

            public ExtraConfigurationProviderConfiguratorTransformation(IProviderConfiguration configuration, Func<TConfigurator, IProviderConfiguration, TConfigurator> extraConfiguration)
            {
                _configuration = configuration;
                _extraConfiguration = extraConfiguration;
            }

            public TConfigurator Transform(TConfigurator configurator) => _extraConfiguration(configurator, _configuration);
        }

        public static ProviderSetup<TConfigurator> Configure<TConfigurator>(this ProviderSetup<TConfigurator> setup, Func<TConfigurator, IProviderConfiguration, TConfigurator> extraConfiguration)
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return new ProviderSetup<TConfigurator>(setup
                .ServiceCollection
                .AddSingleton<IProviderConfiguratorTransformation<TConfigurator>>(s => new ExtraConfigurationProviderConfiguratorTransformation<TConfigurator>(
                    s.GetRequiredService<IProviderConfiguration>(),
                    extraConfiguration)));
        }
    }
}
