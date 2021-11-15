using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.GremlinServer;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup, Action<ProviderSetup<IGremlinServerConfigurator>>? configuration = null)
        {
            return setup
                .UseProvider(
                    "GremlinServer",
                    (source, configuratorTransformation) => source.UseGremlinServer(configuratorTransformation),
                    configuration);
        }

        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<IGremlinServerConfigurator>>? configuration = null)
        {
            return setup
                .UseGremlinServer(configuration)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
