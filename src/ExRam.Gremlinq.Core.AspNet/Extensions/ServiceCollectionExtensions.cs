using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Core.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private sealed class UseLoggerGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly ILogger<GremlinqQueries>? _logger;

            public UseLoggerGremlinQuerySourceTransformation(ILogger<GremlinqQueries>? logger = default)
            {
                _logger = logger;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _logger != null
                    ? source
                        .ConfigureEnvironment(environment => environment
                            .UseLogger(_logger))
                    : source;
            }
        }

        private sealed class ConfigureOptionsGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly IGremlinqConfigurationSection _gremlinqConfigurationSection;

            public ConfigureOptionsGremlinQuerySourceTransformation(IGremlinqConfigurationSection gremlinqConfigurationSection)
            {
                _gremlinqConfigurationSection = gremlinqConfigurationSection;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureOptions(options =>
                        {
                            var loggingSection = _gremlinqConfigurationSection
                                .GetSection("QueryLogging");

                            if (Enum.TryParse<QueryLogVerbosity>(loggingSection["Verbosity"], out var verbosity))
                                options = options.SetValue(GremlinqOption.QueryLogVerbosity, verbosity);

                            if (Enum.TryParse<LogLevel>(loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                                options = options.SetValue(GremlinqOption.QueryLogLogLevel, logLevel);

                            if (Enum.TryParse<Formatting>(loggingSection[$"{nameof(Formatting)}"], out var formatting))
                                options = options.SetValue(GremlinqOption.QueryLogFormatting, formatting);

                            if (Enum.TryParse<GroovyFormatting>(loggingSection[$"{nameof(GroovyFormatting)}"], out var groovyFormatting))
                                options = options.SetValue(GremlinqOption.QueryLogGroovyFormatting, groovyFormatting);

                            return options;
                        }));
            }
        }

        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqSetup> configuration)
        {
            serviceCollection
                .AddSingleton(new GremlinqSetupInfo())
                .AddSingleton<IGremlinqConfigurationSection, GremlinqConfigurationSection>()
                .AddSingleton<IGremlinQuerySourceTransformation, UseLoggerGremlinQuerySourceTransformation>()
                .AddSingleton<IGremlinQuerySourceTransformation, ConfigureOptionsGremlinQuerySourceTransformation>()
                .AddSingleton(c =>
                {
                    var ret = g
                        .ConfigureEnvironment(_ => _);

                    var transformations = c.GetRequiredService<IEnumerable<IGremlinQuerySourceTransformation>>();

                    foreach (var transformation in transformations)
                    {
                        ret = transformation.Transform(ret);
                    }

                    return ret;
                });

            configuration(new GremlinqSetup(serviceCollection));

            return serviceCollection;
        }
    }
}
