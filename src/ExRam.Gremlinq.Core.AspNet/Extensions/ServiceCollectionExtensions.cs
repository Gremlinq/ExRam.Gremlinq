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

        public static IServiceCollection AddGremlinq(this IServiceCollection serviceCollection, Action<GremlinqSetup> configuration)
        {
            serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider
                    .GetRequiredService<IConfiguration>()
                    .GetSection("Gremlinq")))
                .AddSingleton<IGremlinQuerySourceTransformation, UseLoggerGremlinQuerySourceTransformation>()
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
