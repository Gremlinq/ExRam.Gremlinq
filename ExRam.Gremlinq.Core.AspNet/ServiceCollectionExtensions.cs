using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
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

        private sealed class GremlinqConfiguration : IGremlinqConfiguration
        {
            private readonly IConfiguration _baseConfiguration;

            public GremlinqConfiguration(IConfiguration baseConfiguration)
            {
                _baseConfiguration = baseConfiguration;
            }

            public IEnumerable<IConfigurationSection> GetChildren()
            {
                return _baseConfiguration.GetChildren();
            }

            public IChangeToken GetReloadToken()
            {
                return _baseConfiguration.GetReloadToken();
            }

            public IConfigurationSection GetSection(string key)
            {
                return _baseConfiguration.GetSection(key);
            }

            public string this[string key]
            {
                get => _baseConfiguration[key];
                set => _baseConfiguration[key] = value;
            }
        }

        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqOptions> configuration, string? configurationSection = default)
        {
            serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider =>
                {
                    var configuration = serviceProvider.GetService<IConfiguration>();

                    if (configurationSection != null)
                        configuration = configuration.GetSection(configurationSection);

                    return new GremlinqConfiguration(configuration.GetSection("Gremlinq"));
                });

            serviceCollection
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

            serviceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseLoggerGremlinQueryEnvironmentTransformation>();

            configuration(new GremlinqOptions(serviceCollection));

            return serviceCollection;
        }
    }
}
