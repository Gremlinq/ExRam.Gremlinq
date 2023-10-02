// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using Gremlin.Net.Driver;
using System.Net.WebSockets;

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
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
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

        private sealed class ConnectionPoolSettingsGremlinClientFactory : IGremlinClientFactory
        {
            private readonly IGremlinClientFactory _factory;
            private readonly IConfigurationSection _section;

            public ConnectionPoolSettingsGremlinClientFactory(IGremlinClientFactory factory, IConfigurationSection section)
            {
                _factory = factory;
                _section = section;
            }

            public IGremlinClient Create(IGremlinQueryEnvironment environment, GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null)
            {
                if (int.TryParse(_section[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
                    connectionPoolSettings.MaxInProcessPerConnection = maxInProcessPerConnection;

                if (int.TryParse(_section[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
                    connectionPoolSettings.PoolSize = poolSize;

                return _factory.Create(environment, gremlinServer, messageSerializer, connectionPoolSettings, webSocketConfiguration, sessionId);
            }
        }

        public static IGremlinqServicesBuilder<TConfigurator> UseProvider<TConfigurator>(
            this IGremlinqServicesBuilder setup,
            Func<IConfigurableGremlinQuerySource, Func<Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource>> providerChoice)
                where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            setup.Services
                .AddTransient<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    providerChoice,
                    s.GetRequiredService<IEnumerable<IGremlinqConfiguratorTransformation<TConfigurator>>>()));

            return new GremlinqProviderServicesBuilder<TConfigurator>(setup);
        }

        public static IGremlinqServicesBuilder<TConfigurator> ConfigureBase<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            return builder
                .Configure((configurator, section) =>
                {
                    if (section["Alias"] is { Length: > 0 } alias)
                    {
                        configurator = configurator
                            .ConfigureQuerySource(source => source
                                .ConfigureEnvironment(env => env
                                    .ConfigureOptions(options => options
                                        .SetValue(GremlinqOption.Alias, alias))));
                    }

                    return configurator;
                });
        }

        public static IGremlinqServicesBuilder<TConfigurator> ConfigureWebSocket<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
        {
            return builder
                .Configure((configurator, section) =>
                {
                    var authenticationSection = section.GetSection("Authentication");
                    var connectionPoolSection = section.GetSection("ConnectionPool");

                    if (section["Uri"] is { } uri)
                        configurator = configurator.At(uri);

                    configurator
                        .ConfigureClientFactory(factory => new ConnectionPoolSettingsGremlinClientFactory(factory, connectionPoolSection));

                    if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                        configurator = configurator.AuthenticateBy(username, password);

                    return configurator;
                });
        }
    }
}
