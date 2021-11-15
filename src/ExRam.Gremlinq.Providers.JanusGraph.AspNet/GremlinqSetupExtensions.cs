using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup, Action<ProviderSetup<IJanusGraphConfigurator>>? configuration = null)
        {
            return setup
                .UseProvider(
                    "JanusGraph",
                    (source, configuratorTransformation) => source.UseJanusGraph(configuratorTransformation),
                    configuration);
        }

        public static GremlinqSetup UseJanusGraph<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<IJanusGraphConfigurator>>? configuration = null)
        {
            return setup
                .UseJanusGraph(configuration)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
