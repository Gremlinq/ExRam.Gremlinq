// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class ProviderSetupExtensions
    {
        private sealed class SourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _sourceTransformation;

            public SourceTransformation(Func<IGremlinQuerySource, IGremlinQuerySource> sourceTransformation)
            {
                _sourceTransformation = sourceTransformation;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _sourceTransformation(source);
            }
        }

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

        public static ProviderSetup<TProviderConfigurator> Configure<TProviderConfigurator>(this ProviderSetup<TProviderConfigurator> setup, Func<TProviderConfigurator, IProviderConfigurationSection, TProviderConfigurator> extraConfiguration)
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            return new ProviderSetup<TProviderConfigurator>(setup
                .ServiceCollection
                .AddSingleton<IProviderConfiguratorTransformation<TProviderConfigurator>>(serviceProvider => new ExtraConfigurationProviderConfiguratorTransformation<TProviderConfigurator>(
                    serviceProvider.GetRequiredService<IProviderConfigurationSection>(),
                    extraConfiguration)));
        }

        public static ProviderSetup<TProviderConfigurator> Configure<TProviderConfigurator, TProviderConfiguratorTransformation>(this ProviderSetup<TProviderConfigurator> setup)
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
            where TProviderConfiguratorTransformation : class, IProviderConfiguratorTransformation<TProviderConfigurator>
        {
            return new ProviderSetup<TProviderConfigurator>(setup
                .ServiceCollection
                .AddSingleton<IProviderConfiguratorTransformation<TProviderConfigurator>, TProviderConfiguratorTransformation>());
        }

        public static ProviderSetup<TProviderConfigurator> ConfigureQuerySource<TProviderConfigurator>(this ProviderSetup<TProviderConfigurator> setup, Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation)
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            setup.ServiceCollection.AddSingleton<IGremlinQuerySourceTransformation>(new SourceTransformation(sourceTranformation));

            return setup;
        }
    }
}
