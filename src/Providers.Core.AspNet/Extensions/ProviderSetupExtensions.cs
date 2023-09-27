using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;
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

        public static ProviderSetup<TConfigurator> Configure<TConfigurator>(this ProviderSetup<TConfigurator> setup)
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return setup
                .Configure((configurator, providerSection) =>
                {
                    if (providerSection["Alias"] is { Length: > 0 } alias)
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

        public static TConfigurator ConfigureWebSocket<TConfigurator>(this TConfigurator configurator, IConfigurationSection section)
           where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
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
        }

        public static ProviderSetup<TConfigurator> Configure<TConfigurator>(this ProviderSetup<TConfigurator> setup, Func<TConfigurator, IProviderConfigurationSection, TConfigurator> extraConfiguration)
           where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return setup
                .Configure((configurator, _, providerSection) => extraConfiguration(configurator, providerSection.MergeWithGremlinqSection()));
        }

        public static ProviderSetup<TConfigurator> Configure<TConfigurator>(this ProviderSetup<TConfigurator> setup, Func<TConfigurator, IGremlinqConfigurationSection, IProviderConfigurationSection, TConfigurator> extraConfiguration)
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return new ProviderSetup<TConfigurator>(setup
                .ServiceCollection
                .AddSingleton<IProviderConfiguratorTransformation<TConfigurator>>(serviceProvider => new ExtraConfigurationProviderConfiguratorTransformation<TConfigurator>(
                    serviceProvider.GetRequiredService<IGremlinqConfigurationSection>(),
                    serviceProvider.GetRequiredService<IProviderConfigurationSection>(),
                    extraConfiguration)));
        }
    }
}
