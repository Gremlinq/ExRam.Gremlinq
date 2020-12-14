using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class ConfigureWebSocketGremlinQueryExecutorBuilderTransformation : IWebSocketGremlinQueryExecutorBuilderTransformation
        {
            private readonly Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> _transformation;

            public ConfigureWebSocketGremlinQueryExecutorBuilderTransformation(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation)
            {
                _transformation = transformation;
            }

            public IWebSocketGremlinQueryExecutorBuilder Transform(IWebSocketGremlinQueryExecutorBuilder builder)
            {
                return _transformation(builder);
            }
        }

        private sealed class ConfigureLoggingGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _loggingSection;

            public ConfigureLoggingGremlinQueryEnvironmentTransformation(IGremlinqConfiguration configuration)
            {
                _loggingSection = configuration
                    .GetSection("QueryLogging");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .ConfigureOptions(options =>
                    {
                        if (Enum.TryParse<QueryLogVerbosity>(_loggingSection["Verbosity"], out var verbosity))
                            options = options.SetValue(WebSocketGremlinqOptions.QueryLogVerbosity, verbosity);

                        if (Enum.TryParse<LogLevel>(_loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                            options = options.SetValue(WebSocketGremlinqOptions.QueryLogLogLevel, logLevel);

                        if (Enum.TryParse<Formatting>(_loggingSection[$"{nameof(Formatting)}"], out var formatting))
                            options = options.SetValue(WebSocketGremlinqOptions.QueryLogFormatting, formatting);

                        return options;
                    });
            }
        }

        public static GremlinqSetup UseWebSocket(this GremlinqSetup setup)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQueryEnvironmentTransformation, ConfigureLoggingGremlinQueryEnvironmentTransformation>());
        }

        public static GremlinqSetup ConfigureWebSocketBuilder(this GremlinqSetup setup, Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IWebSocketGremlinQueryExecutorBuilderTransformation>(_ => new ConfigureWebSocketGremlinQueryExecutorBuilderTransformation(transformation)));
        }
    }
}
