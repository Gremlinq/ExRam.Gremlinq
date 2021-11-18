using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup, Action<ProviderSetup<IJanusGraphConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "JanusGraph",
                    (source, configuratorTransformation) => source
                        .UseJanusGraph(configuratorTransformation),
                    setup => setup
                        .ConfigureWebSocket(),
                    extraSetupAction);
        }

        public static GremlinqSetup UseJanusGraph<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<IJanusGraphConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseJanusGraph(extraSetupAction)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
