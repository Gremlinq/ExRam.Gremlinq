using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfigurationBuilderExtensions
    {
        public static IWebSocketConfigurationBuilder Configure(this IWebSocketConfigurationBuilder builder, IConfiguration configuration)
        {
            var loggingSection = configuration.GetSection("QueryLoggingg");

            return builder
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
                });
        }
    }

    public static class ServiceCollectionExtensions
    {
        private sealed class UseGremlinServerGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;

            public UseGremlinServerGremlinQueryEnvironmentTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("GremlinServer");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseGremlinServer(builder => builder.Configure(_configuration));
            }
        }

        public static GremlinqOptions UseGremlinServer(this GremlinqOptions options)
        {
            return new GremlinqOptions(options.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseGremlinServerGremlinQueryEnvironmentTransformation>());
        }
    }
}
