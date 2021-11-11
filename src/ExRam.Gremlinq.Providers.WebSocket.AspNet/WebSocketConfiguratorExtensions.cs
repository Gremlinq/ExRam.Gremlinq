using System;
using System.Net.WebSockets;

using ExRam.Gremlinq.Core.Serialization;
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

            public GremlinClient Create(GremlinServer gremlinServer, IMessageSerializer? messageSerializer = null, ConnectionPoolSettings? connectionPoolSettings = null, Action<ClientWebSocketOptions>? webSocketConfiguration = null, string? sessionId = null)
            {
                if (int.TryParse(_section[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
                    (connectionPoolSettings ??= new ConnectionPoolSettings()).MaxInProcessPerConnection = maxInProcessPerConnection;

                if (int.TryParse(_section[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
                    (connectionPoolSettings ??= new ConnectionPoolSettings()).PoolSize = poolSize;

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
                .ConfigureGremlinClientFactory(factory => new ConnectionPoolSettingsGremlinClientFactory(factory, connectionPoolSection));

            if (configuration["Alias"] is { } alias)
                webSocketConfigurator = webSocketConfigurator.SetAlias(alias);

            if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                webSocketConfigurator = webSocketConfigurator.AuthenticateBy(username, password);

            //If needed: Configure by type name.
            //if (Enum.TryParse<SerializationFormat>(configuration[$"{nameof(SerializationFormat)}"], out var graphsonVersion))
            //    webSocketConfigurator = webSocketConfigurator.SetSerializationFormat(graphsonVersion);

            return webSocketConfigurator;
        }
    }
}
