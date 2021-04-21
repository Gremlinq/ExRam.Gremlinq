using System;
using ExRam.Gremlinq.Providers.GremlinServer;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup, Func<IGremlinServerConfigurator, IConfiguration, IGremlinServerConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseProvider(
                    "GremlinServer",
                    (e, f) => e.UseGremlinServer(f),
                    (configurator, _) => configurator,
                    extraConfiguration);
        }

        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup, Func<IGremlinServerConfigurator, IConfiguration, IGremlinServerConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseGremlinServer(extraConfiguration)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
