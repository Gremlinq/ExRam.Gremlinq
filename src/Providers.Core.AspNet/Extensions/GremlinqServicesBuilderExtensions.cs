// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        private sealed class GremlinqProviderServicesBuilder<TConfigurator> : IGremlinqServicesBuilder<TConfigurator>
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private sealed class ExtraConfigurationProviderConfiguratorTransformation : IGremlinqConfiguratorTransformation<TConfigurator>
            {
                private readonly IEffectiveGremlinqConfigurationSection _section;
                private readonly Func<TConfigurator, IConfigurationSection, TConfigurator> _extraConfiguration;

                public ExtraConfigurationProviderConfiguratorTransformation(IEffectiveGremlinqConfigurationSection providerSection, Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration)
                {
                    _section = providerSection;
                    _extraConfiguration = extraConfiguration;
                }

                public TConfigurator Transform(TConfigurator configurator) => _extraConfiguration(configurator, _section);
            }

            private readonly IGremlinqServicesBuilder _baseSetup;

            public GremlinqProviderServicesBuilder(IGremlinqServicesBuilder baseSetup)
            {
                _baseSetup = baseSetup;
            }

            public IGremlinqServicesBuilder<TConfigurator> FromProviderSection(string sectionName)
            {
                Services
                    .AddSingleton(s => new ProviderConfigurationSection<TConfigurator>(s.GetRequiredService<IGremlinqConfigurationSection>(), sectionName))
                    .AddSingleton<IProviderConfigurationSection>(s => s.GetRequiredService<ProviderConfigurationSection<TConfigurator>>())
                    .TryAddTransient<IEffectiveGremlinqConfigurationSection>(s => s.GetRequiredService<ProviderConfigurationSection<TConfigurator>>());

                return this;
            }

            public IGremlinqServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration)
            {
                Services
                    .AddTransient<IGremlinqConfiguratorTransformation<TConfigurator>>(serviceProvider => new ExtraConfigurationProviderConfiguratorTransformation(
                        serviceProvider.GetRequiredService<IEffectiveGremlinqConfigurationSection>(),
                        extraConfiguration));

                return this;
            }

            public IGremlinqServicesBuilder<TConfigurator> Configure<TProviderConfiguratorTransformation>()
                where TProviderConfiguratorTransformation : class, IGremlinqConfiguratorTransformation<TConfigurator>
            {
                Services
                    .AddTransient<IGremlinqConfiguratorTransformation<TConfigurator>, TProviderConfiguratorTransformation>();

                return this;
            }

            public IGremlinqServicesBuilder ConfigureQuerySource(Func<IGremlinQuerySource, IConfigurationSection, IGremlinQuerySource> sourceTranformation) => _baseSetup.ConfigureQuerySource(sourceTranformation);

            public IGremlinqServicesBuilder FromBaseSection(string sectionName) => _baseSetup.FromBaseSection(sectionName);

            public IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
                where  TTransformation : class, IGremlinQuerySourceTransformation => _baseSetup.ConfigureQuerySource<TTransformation>();

            public IServiceCollection Services => _baseSetup.Services;
        }

        private sealed class UseProviderGremlinQuerySourceTransformation<TConfigurator> : IGremlinQuerySourceTransformation
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private readonly IEnumerable<IGremlinqConfiguratorTransformation<TConfigurator>> _providerConfiguratorTransformations;
            private readonly Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> _providerChoice;

            public UseProviderGremlinQuerySourceTransformation(
                Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice,
                IEnumerable<IGremlinqConfiguratorTransformation<TConfigurator>> providerConfiguratorTransformations)
            {
                _providerChoice = providerChoice;
                _providerConfiguratorTransformations = providerConfiguratorTransformations;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _providerChoice
                .Invoke(source)
                .Invoke(configurator =>
                {
                    foreach (var transformation in _providerConfiguratorTransformations)
                    {
                        configurator = transformation.Transform(configurator);
                    }

                    return configurator;
                });
        }

        public static IGremlinqServicesBuilder<TConfigurator> UseProvider<TConfigurator>(
            this IGremlinqServicesBuilder setup,
            Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice)
                where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            setup.Services
                .AddTransient<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    providerChoice,
                    s.GetRequiredService<IEnumerable<IGremlinqConfiguratorTransformation<TConfigurator>>>()))
                .AddSingleton<IProviderConfigurationSection, ProviderConfigurationSection<TConfigurator>>();

            return new GremlinqProviderServicesBuilder<TConfigurator>(setup);
        }
    }
}
