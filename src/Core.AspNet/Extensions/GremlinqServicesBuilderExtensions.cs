using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        private sealed class GremlinqProviderServicesBuilder<TConfigurator> : IGremlinqServicesBuilder<TConfigurator>
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            private sealed class ExtraConfigurationProviderConfiguratorTransformation : IGremlinqConfiguratorTransformation<TConfigurator>
            {
                private readonly IGremlinqConfigurationSection _section;
                private readonly Func<TConfigurator, IConfigurationSection, TConfigurator> _extraConfiguration;

                public ExtraConfigurationProviderConfiguratorTransformation(IGremlinqConfigurationSection providerSection, Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration)
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

            public IGremlinqServicesBuilder<TConfigurator> Configure(Func<TConfigurator, IConfigurationSection, TConfigurator> extraConfiguration)
            {
                Services
                    .AddTransient<IGremlinqConfiguratorTransformation<TConfigurator>>(serviceProvider => new ExtraConfigurationProviderConfiguratorTransformation(
                        serviceProvider.GetRequiredService<IGremlinqConfigurationSection>(),
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
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            private readonly IEnumerable<IGremlinqConfiguratorTransformation<TConfigurator>> _providerConfiguratorTransformations;
            private readonly Func<IGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> _providerChoice;

            public UseProviderGremlinQuerySourceTransformation(
                Func<IGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice,
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

        public static IGremlinqServicesBuilder ConfigureBase(this IGremlinqServicesBuilder builder) => builder
            .ConfigureQuerySource((source, section) =>
            {
                if (section["Alias"] is { Length: > 0 } alias)
                {
                    return source
                        .ConfigureEnvironment(env => env
                            .ConfigureOptions(options => options
                                .SetValue(GremlinqOption.Alias, alias)));
                }

                return source;
            });

        public static IGremlinqServicesBuilder<TConfigurator> UseProvider<TConfigurator>(
            this IGremlinqServicesBuilder setup,
            Func<IGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice)
                where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            setup.Services
                .AddTransient<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    providerChoice,
                    s.GetRequiredService<IEnumerable<IGremlinqConfiguratorTransformation<TConfigurator>>>()));

            return new GremlinqProviderServicesBuilder<TConfigurator>(setup);
        }
    }
}
