// ReSharper disable HeapView.PossibleBoxingAllocation
using System.Net.WebSockets;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public static class ProviderSetupExtensions
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

            public IGremlinClient Create(GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null)
            {
                if (int.TryParse(_section[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
                    connectionPoolSettings.MaxInProcessPerConnection = maxInProcessPerConnection;

                if (int.TryParse(_section[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
                    connectionPoolSettings.PoolSize = poolSize;

                return _factory.Create(gremlinServer, messageSerializer, connectionPoolSettings, webSocketConfiguration, sessionId);
            }
        }

        public static ProviderSetup<TConfigurator> ConfigureWebSocket<TConfigurator>(this ProviderSetup<TConfigurator> setup)
           where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
        {
            return setup
                .Configure((configurator, providerSection) =>
                {
                    var authenticationSection = providerSection.GetSection("Authentication");
                    var connectionPoolSection = providerSection.GetSection("ConnectionPool");

                    if (providerSection["Uri"] is { } uri)
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
