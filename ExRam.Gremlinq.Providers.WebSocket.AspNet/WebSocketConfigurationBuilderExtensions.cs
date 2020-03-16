using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfigurationBuilderExtensions
    {
        public static IWebSocketConfigurationBuilder Configure(this IWebSocketConfigurationBuilder builder, IConfiguration configuration)
        {
            var loggingSection = configuration.GetSection("QueryLogging");
            var authenticationSection = configuration.GetSection("Authentication");
            var connectionPoolSection = configuration.GetSection("ConnectionPool");

            builder = builder
                .At(configuration.GetRequiredConfiguration("Uri"))
                .ConfigureQueryLoggingOptions(o =>
                {
                    if (Enum.TryParse<QueryLoggingVerbosity>(loggingSection["Verbosity"], out var verbosity))
                        o = o.SetQueryLoggingVerbosity(verbosity);

                    if (Enum.TryParse<LogLevel>(loggingSection["LogLevel"], out var logLevel))
                        o = o.SetLogLevel(logLevel);

                    if (Enum.TryParse<Formatting>(loggingSection["LogLevel"], out var formatting))
                        o = o.SetFormatting(formatting);

                    return o;
                })
                .ConfigureConnectionPool(connectionPoolSettings =>
                {
                    if (int.TryParse(connectionPoolSection["MaxInProcessPerConnection"], out var maxInProcessPerConnection))
                        connectionPoolSettings.MaxInProcessPerConnection = maxInProcessPerConnection;

                    if (int.TryParse(connectionPoolSection["PoolSize"], out var poolSize))
                        connectionPoolSettings.PoolSize = poolSize;
                });

            if (configuration["Alias"] is { } alias)
                builder = builder.SetAlias(alias);

            if (authenticationSection["Username"] is { } username && authenticationSection["Password"] is { } password)
                builder = builder.AuthenticateBy(username, password);

            if (Enum.TryParse<GraphsonVersion>(loggingSection["GraphsonVersion"], out var graphsonVersion))
                builder = builder.SetGraphSONVersion(graphsonVersion);

            return builder;
        }
    }
}
