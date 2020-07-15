using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private sealed class UseLoggerGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly ILogger<GremlinqQueries>? _logger;

            public UseLoggerGremlinQueryEnvironmentTransformation(ILogger<GremlinqQueries>? logger = default)
            {
                _logger = logger;
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return _logger != null
                    ? environment.UseLogger(_logger)
                    : environment;
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
                            options = options.SetValue(GremlinqOption.QueryLogVerbosity, verbosity);

                        if (Enum.TryParse<LogLevel>(_loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                            options = options.SetValue(GremlinqOption.QueryLogLogLevel, logLevel);

                        if (Enum.TryParse<Formatting>(_loggingSection[$"{nameof(Formatting)}"], out var formatting))
                            options = options.SetValue(GremlinqOption.QueryLogFormatting, formatting);

                        return options;
                    });
            }
        }

        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqSetup> configuration)
        {
            serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider.GetService<IConfiguration>().GetSection("Gremlinq")))
                .AddSingleton<IGremlinQueryEnvironmentTransformation, UseLoggerGremlinQueryEnvironmentTransformation>()
                .AddSingleton<IGremlinQueryEnvironmentTransformation, ConfigureLoggingGremlinQueryEnvironmentTransformation>()
                .AddSingleton(c =>
                {
                    var transformations = c.GetService<IEnumerable<IGremlinQueryEnvironmentTransformation>>();

                    return g
                        .ConfigureEnvironment(env =>
                        {
                            foreach (var transformation in transformations)
                            {
                                env = transformation.Transform(env);
                            }

                            return env;
                        });
                });

            configuration(new GremlinqSetup(serviceCollection));

            return serviceCollection;
        }
    }
}
