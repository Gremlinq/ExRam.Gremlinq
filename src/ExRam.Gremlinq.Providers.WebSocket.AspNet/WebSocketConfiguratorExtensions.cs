using System;
using System.Net.WebSockets;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfiguratorExtensions
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

        public static IWebSocketConfigurator ConfigureFrom(
            this IWebSocketConfigurator webSocketConfigurator,
            IConfiguration configuration)
        {
            var authenticationSection = configuration.GetSection("Authentication");
            var connectionPoolSection = configuration.GetSection("ConnectionPool");

            if (configuration["Uri"] is { } uri)
                webSocketConfigurator = webSocketConfigurator.At(uri);

            webSocketConfigurator
                .ConfigureClientFactory(factory => new ConnectionPoolSettingsGremlinClientFactory(factory, connectionPoolSection));

            if (configuration["Alias"] is { } alias)
                webSocketConfigurator = webSocketConfigurator.SetAlias(alias);

            if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                webSocketConfigurator = webSocketConfigurator.AuthenticateBy(username, password);

            return webSocketConfigurator;
        }
    }
}
