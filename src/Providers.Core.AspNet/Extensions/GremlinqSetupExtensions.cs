// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class GremlinqProviderSetup<TConfigurator> : IGremlinqProviderSetup<TConfigurator>
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

            private readonly IGremlinqSetup _baseSetup;

            public GremlinqProviderSetup(IGremlinqSetup baseSetup)
            {
                _baseSetup = baseSetup;
            }

            public IGremlinqProviderSetup<TConfigurator> Configure(Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration)
            {
                Services
                    .AddTransient<IProviderConfiguratorTransformation<TConfigurator>>(serviceProvider => new ExtraConfigurationProviderConfiguratorTransformation(
                        serviceProvider.GetRequiredService<IProviderConfigurationSection>(),
                        extraConfiguration));

                return this;
            }

            public IGremlinqProviderSetup<TConfigurator> Configure<TProviderConfiguratorTransformation>()
                where TProviderConfiguratorTransformation : class, IProviderConfiguratorTransformation<TConfigurator>
            {
                Services
                    .AddTransient<IProviderConfiguratorTransformation<TConfigurator>, TProviderConfiguratorTransformation>();

                return this;
            }

            public IGremlinqSetup ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation) => _baseSetup.ConfigureQuerySource(sourceTranformation);

            public IGremlinqSetup UseConfigurationSection(string sectionName) => _baseSetup.UseConfigurationSection(sectionName);

            public IGremlinqSetup ConfigureQuerySource<TTransformation>()
                where  TTransformation : class, IGremlinQuerySourceTransformation => _baseSetup.ConfigureQuerySource<TTransformation>();

            public IServiceCollection Services => throw new NotImplementedException();
        }

        private sealed class UseProviderGremlinQuerySourceTransformation<TProviderConfigurator> : IGremlinQuerySourceTransformation
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            private readonly IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> _providerConfiguratorTransformations;
            private readonly Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> _providerChoice;

            public UseProviderGremlinQuerySourceTransformation(
                Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
                IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> providerConfiguratorTransformations)
            {
                _providerChoice = providerChoice;
                _providerConfiguratorTransformations = providerConfiguratorTransformations;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _providerChoice(
                source,
                configurator =>
                {
                    foreach (var transformation in _providerConfiguratorTransformations)
                    {
                        configurator = transformation.Transform(configurator);
                    }

                    return configurator;
                });
        }

        public static IGremlinqProviderSetup<TConfigurator> UseProvider<TConfigurator>(
            this IGremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice)
                where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            setup.Services
                .AddTransient<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    providerChoice,
                    s.GetRequiredService<IEnumerable<IProviderConfiguratorTransformation<TConfigurator>>>()))
                .AddSingleton<IProviderConfigurationSection>(s => new ProviderConfigurationSection<TConfigurator>(s.GetRequiredService<IGremlinqConfigurationSection>(), sectionName));

            return new GremlinqProviderSetup<TConfigurator>(setup);
        }
    }
}
