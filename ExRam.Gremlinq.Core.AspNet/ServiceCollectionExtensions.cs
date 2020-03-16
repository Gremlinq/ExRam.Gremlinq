using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class ServiceCollectionExtensions
    {
        private struct GremlinqQueries
        {
        }

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

        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqOptions> configuration)
        {
            serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider.GetService<IConfiguration>().GetSection("Gremlinq")))
                .AddSingleton<IGremlinQueryEnvironmentTransformation, UseLoggerGremlinQueryEnvironmentTransformation>()
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

            configuration(new GremlinqOptions(serviceCollection));

            return serviceCollection;
        }
    }
}
