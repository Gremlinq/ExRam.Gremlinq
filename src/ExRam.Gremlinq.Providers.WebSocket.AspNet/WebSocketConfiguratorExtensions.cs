using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfiguratorExtensions
    {
        public static IWebSocketConfigurator Configure(
            this IWebSocketConfigurator configurator,
            IConfiguration configuration)
        {
            var authenticationSection = configuration.GetSection("Authentication");
            var connectionPoolSection = configuration.GetSection("ConnectionPool");

            configurator = configurator
                .At(configuration.GetRequiredConfiguration("Uri"))
                .ConfigureConnectionPool(connectionPoolSettings =>
                {
                    if (int.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
                        connectionPoolSettings.MaxInProcessPerConnection = maxInProcessPerConnection;

                    if (int.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
                        connectionPoolSettings.PoolSize = poolSize;
                });

            if (configuration["Alias"] is { } alias)
                configurator = configurator.SetAlias(alias);

            if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                configurator = configurator.AuthenticateBy(username, password);

            if (Enum.TryParse<SerializationFormat>(configuration[$"{nameof(SerializationFormat)}"], out var graphsonVersion))
                configurator = configurator.SetSerializationFormat(graphsonVersion);

            return configurator;
        }
    }
}
