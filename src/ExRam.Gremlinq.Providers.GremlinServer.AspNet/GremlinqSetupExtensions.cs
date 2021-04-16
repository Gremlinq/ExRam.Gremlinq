using System;
using ExRam.Gremlinq.Providers.GremlinServer;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup)
        {
            return setup
                .UseProvider<IGremlinServerConfigurator>(
                    "GremlinServer",
                    (e, f) => e.UseGremlinServer(f),
                    (configurator, configuration) => configurator
                        .At(new Uri(configuration.GetRequiredConfiguration("Uri"))));
        }

        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseGremlinServer()
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
