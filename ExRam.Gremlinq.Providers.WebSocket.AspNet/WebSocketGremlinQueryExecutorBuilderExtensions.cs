using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketGremlinQueryExecutorBuilderExtensions
    {
        public static IWebSocketGremlinQueryExecutorBuilder Configure(
            this IWebSocketGremlinQueryExecutorBuilder builder,
            IConfiguration configuration)
        {
            var authenticationSection = configuration.GetSection("Authentication");
            var connectionPoolSection = configuration.GetSection("ConnectionPool");

            builder = builder
                .At(configuration.GetRequiredConfiguration("Uri"))
                .ConfigureConnectionPool(connectionPoolSettings =>
                {
                    if (int.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
                        connectionPoolSettings.MaxInProcessPerConnection = maxInProcessPerConnection;

                    if (int.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
                        connectionPoolSettings.PoolSize = poolSize;
                });

            if (configuration["Alias"] is { } alias)
                builder = builder.SetAlias(alias);

            if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                builder = builder.AuthenticateBy(username, password);

            if (Enum.TryParse<SerializationFormat>(configuration[$"{nameof(SerializationFormat)}"], out var graphsonVersion))
                builder = builder.SetSerializationFormat(graphsonVersion);

            return builder;
        }

        public static IWebSocketGremlinQueryExecutorBuilder Transform(
            this IWebSocketGremlinQueryExecutorBuilder builder,
            IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> webSocketTransformations)
        {
            foreach (var webSocketTransformation in webSocketTransformations)
            {
                builder = webSocketTransformation.Transform(builder);
            }

            return builder;
        }
    }
}
