// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using Gremlin.Net.Driver;
using System.Net.WebSockets;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
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
