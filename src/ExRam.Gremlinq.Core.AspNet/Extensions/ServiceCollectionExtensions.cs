using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Core.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqSetup> configuration)
        {
            serviceCollection
                .TryAddSingleton(new GremlinqSetupInfo());

            serviceCollection
                .TryAddSingleton<IGremlinqConfigurationSection, GremlinqConfigurationSection>();

            serviceCollection
                .TryAddSingleton(serviceProvider =>
                {
                    var querySource = g
                        .ConfigureEnvironment(environment =>
                        {
                            if (serviceProvider.GetService<ILogger<GremlinqQueries>>() is { } logger)
                            {
                                environment = environment
                                    .UseLogger(logger);
                            }

                            if (serviceProvider.GetService<IGremlinqConfigurationSection>() is { } gremlinConfigSection)
                            {
                                environment = environment
                                    .ConfigureOptions(options =>
                                    {
                                        var loggingSection = gremlinConfigSection
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
                                    });
                            }

                            return environment;
                        });

                    if (serviceProvider.GetService<IEnumerable<IGremlinQuerySourceTransformation>>() is { } transformations)
                    {
                        foreach (var transformation in transformations)
                        {
                            querySource = transformation.Transform(querySource);
                        }
                    }

                    return querySource;
                });

            configuration(new GremlinqSetup(serviceCollection));

            return serviceCollection;
        }
    }
}
