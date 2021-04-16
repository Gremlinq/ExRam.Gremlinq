using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfiguratorExtensions
    {
        public static IWebSocketConfigurator ConfigureFrom(
            this IWebSocketConfigurator webSocketConfigurator,
            IConfiguration configuration)
        {
            var authenticationSection = configuration.GetSection("Authentication");
            var connectionPoolSection = configuration.GetSection("ConnectionPool");

            if (configuration["Uri"] is { } uri)
                webSocketConfigurator = webSocketConfigurator.At(uri);

            webSocketConfigurator
                .ConfigureConnectionPool(connectionPoolSettings =>
                {
                    if (int.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
                        connectionPoolSettings.MaxInProcessPerConnection = maxInProcessPerConnection;

                    if (int.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
                        connectionPoolSettings.PoolSize = poolSize;
                });

            if (configuration["Alias"] is { } alias)
                webSocketConfigurator = webSocketConfigurator.SetAlias(alias);

            if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                webSocketConfigurator = webSocketConfigurator.AuthenticateBy(username, password);

            if (Enum.TryParse<SerializationFormat>(configuration[$"{nameof(SerializationFormat)}"], out var graphsonVersion))
                webSocketConfigurator = webSocketConfigurator.SetSerializationFormat(graphsonVersion);

            return webSocketConfigurator;
        }
    }
}
