using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketGremlinQueryEnvironmentBuilderExtensions
    {
        private sealed class LogConfigurationWebSocketGremlinQueryEnvironmentBuilder : IWebSocketGremlinQueryEnvironmentBuilder
        {
            private readonly IConfiguration _configuration;
            private readonly IWebSocketGremlinQueryEnvironmentBuilder _baseBuilder;

            public LogConfigurationWebSocketGremlinQueryEnvironmentBuilder(
                IWebSocketGremlinQueryEnvironmentBuilder baseBuilder,
                IConfiguration configuration)
            {
                _baseBuilder = baseBuilder;
                _configuration = configuration;
            }

            IGremlinQueryEnvironment IGremlinQueryEnvironmentBuilder.Build()
            {
                var loggingSection = _configuration.GetSection("QueryLogging");

                return _baseBuilder
                    .Build()
                    .ConfigureOptions(options =>
                    {
                        if (Enum.TryParse<QueryLogVerbosity>(loggingSection["Verbosity"], out var verbosity))
                            options = options.SetValue(GremlinqOption.QueryLogVerbosity, verbosity);

                        if (Enum.TryParse<LogLevel>(loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                            options = options.SetValue(GremlinqOption.QueryLogLogLevel, logLevel);

                        if (Enum.TryParse<Formatting>(loggingSection[$"{nameof(Formatting)}"], out var formatting))
                            options = options.SetValue(GremlinqOption.QueryLogFormatting, formatting);

                        return options;
                    });
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.At(Uri uri)
            {
                return _baseBuilder.At(uri);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.AuthenticateBy(string username, string password)
            {
                return _baseBuilder.AuthenticateBy(username, password);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.SetAlias(string alias)
            {
                return _baseBuilder.SetAlias(alias);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation)
            {
                return _baseBuilder.ConfigureConnectionPool(transformation);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation)
            {
                return _baseBuilder.ConfigureGremlinClient(transformation);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.SetSerializationFormat(SerializationFormat version)
            {
                return _baseBuilder.SetSerializationFormat(version);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.AddGraphSONSerializer(Type type, IGraphSONSerializer serializer)
            {
                return _baseBuilder.AddGraphSONSerializer(type, serializer);
            }

            IWebSocketGremlinQueryEnvironmentBuilder IWebSocketGremlinQueryEnvironmentBuilder.AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer)
            {
                return _baseBuilder.AddGraphSONDeserializer(typename, serializer);
            }
        }

        public static IWebSocketGremlinQueryEnvironmentBuilder Configure(
            this IWebSocketGremlinQueryEnvironmentBuilder builder,
            IConfiguration configuration,
            IEnumerable<IWebSocketGremlinQueryEnvironmentBuilderTransformation> webSocketTransformations)
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

            foreach (var webSocketTransformation in webSocketTransformations)
            {
                builder = webSocketTransformation.Transform(builder);
            }

            return new LogConfigurationWebSocketGremlinQueryEnvironmentBuilder(builder, configuration);
        }
    }
}
