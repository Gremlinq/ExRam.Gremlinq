// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Driver;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal static class GremlinqConfiguratorExtensions
    {
        public static TConfigurator ConfigureWebSocket<TConfigurator>(this TConfigurator configurator, IConfigurationSection section)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            var connectionPoolSection = section.GetSection("ConnectionPool");

            if (section["Uri"] is { } uri)
                configurator = configurator.At(uri);

            if (uint.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.MaxInProcessPerConnection)}"], out var maxInProcessPerConnection))
            {
                configurator = configurator
                    .ConfigureClientFactory(factory => factory
                        .WithMaxInProcessPerConnection(maxInProcessPerConnection));
            }

            if (uint.TryParse(connectionPoolSection[$"{nameof(ConnectionPoolSettings.PoolSize)}"], out var poolSize))
            {
                configurator = configurator
                    .ConfigureClientFactory(factory => factory
                        .WithPoolSize(poolSize));
            }

            return configurator;
        }

        public static TConfigurator ConfigureBasicAuthentication<TConfigurator>(this TConfigurator configurator, IConfigurationSection section)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            var authenticationSection = section.GetSection("Authentication");

            return authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password
                ? configurator.AuthenticateBy(username, password)
                : configurator;
        }
    }
}
