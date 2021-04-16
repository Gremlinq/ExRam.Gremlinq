using System;
using ExRam.Gremlinq.Providers.JanusGraph;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup)
        {
            return setup
                .UseProvider<IJanusGraphConfigurator>(
                    "JanusGraph",
                    (e, f) => e.UseJanusGraph(f),
                    (configurator, configuration) => configurator
                        .At(new Uri(configuration.GetRequiredConfiguration("Uri"))));
        }

        public static GremlinqSetup UseJanusGraph<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseJanusGraph()
                .UseModel(GraphModel
                    .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()));
        }
    }
}
