// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        private sealed class GremlinqProviderServicesBuilder<TConfigurator> : IGremlinqProviderServicesBuilder<TConfigurator>
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private sealed class ExtraConfigurationProviderConfiguratorTransformation : IProviderConfiguratorTransformation<TConfigurator>
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

            private readonly IGremlinqServicesBuilder _baseSetup;

            public GremlinqProviderServicesBuilder(IGremlinqServicesBuilder baseSetup)
            {
                _baseSetup = baseSetup;
            }

            public IGremlinqProviderServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration)
            {
                Services
                    .AddTransient<IProviderConfiguratorTransformation<TConfigurator>>(serviceProvider => new ExtraConfigurationProviderConfiguratorTransformation(
                        serviceProvider.GetRequiredService<IProviderConfigurationSection>(),
                        extraConfiguration));

                return this;
            }

            public IGremlinqProviderServicesBuilder<TConfigurator> Configure<TProviderConfiguratorTransformation>()
                where TProviderConfiguratorTransformation : class, IProviderConfiguratorTransformation<TConfigurator>
            {
                Services
                    .AddTransient<IProviderConfiguratorTransformation<TConfigurator>, TProviderConfiguratorTransformation>();

                return this;
            }

            public IGremlinqServicesBuilder ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation) => _baseSetup.ConfigureQuerySource(sourceTranformation);

            public IGremlinqServicesBuilder UseConfigurationSection(string sectionName) => _baseSetup.UseConfigurationSection(sectionName);

            public IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
                where  TTransformation : class, IGremlinQuerySourceTransformation => _baseSetup.ConfigureQuerySource<TTransformation>();

            public IServiceCollection Services => _baseSetup.Services;
        }

        private sealed class UseProviderGremlinQuerySourceTransformation<TConfigurator> : IGremlinQuerySourceTransformation
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private readonly IEnumerable<IProviderConfiguratorTransformation<TConfigurator>> _providerConfiguratorTransformations;
            private readonly Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> _providerChoice;

            public UseProviderGremlinQuerySourceTransformation(
                Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice,
                IEnumerable<IProviderConfiguratorTransformation<TConfigurator>> providerConfiguratorTransformations)
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

        public static IGremlinqProviderServicesBuilder<TConfigurator> UseProvider<TConfigurator>(
            this IGremlinqServicesBuilder setup,
            Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice)
                where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            setup.Services
                .AddTransient<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    providerChoice,
                    s.GetRequiredService<IEnumerable<IProviderConfiguratorTransformation<TConfigurator>>>()));

            return new GremlinqProviderServicesBuilder<TConfigurator>(setup);
        }
    }
}
