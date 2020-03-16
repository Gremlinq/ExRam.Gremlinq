using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class ServiceCollectionExtensions
    {
        private sealed class UseGremlinServerGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IGremlinqConfiguration _configuration;

            public UseGremlinServerGremlinQueryEnvironmentTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration;
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseGremlinServer(builder =>
                    {
                        builder = builder
                            .At(_configuration.GetRequiredConfiguration("GremlinServer:Uri"));

                        if (Enum.TryParse<QueryLoggingVerbosity>(_configuration?["GremlinServer:QueryLoggingVerbosity"], out var verbosity))
                        {
                            builder = builder.ConfigureQueryLoggingOptions(o => o
                                .SetQueryLoggingVerbosity(verbosity));
                        }

                        return builder;
                    });
            }
        }

        public static GremlinqOptions UseGremlinServer(this GremlinqOptions options)
        {
            return new GremlinqOptions(options.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseGremlinServerGremlinQueryEnvironmentTransformation>());
        }
    }
}
