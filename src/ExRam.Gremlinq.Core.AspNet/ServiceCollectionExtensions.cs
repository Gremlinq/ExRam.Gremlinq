using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqSetup> configuration)
        {
            serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider.GetRequiredService<IConfiguration>().GetSection("Gremlinq")))
                .AddSingleton<IGremlinQueryEnvironmentTransformation, UseLoggerGremlinQueryEnvironmentTransformation>()
                .AddSingleton(c =>
                {
                    var transformations = c.GetRequiredService<IEnumerable<IGremlinQueryEnvironmentTransformation>>();

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
