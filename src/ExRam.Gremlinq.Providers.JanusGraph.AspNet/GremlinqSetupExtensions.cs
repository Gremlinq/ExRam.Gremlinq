using ExRam.Gremlinq.Providers.JanusGraph;

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
                    (configurator, _) => configurator);
        }

        public static GremlinqSetup UseJanusGraph<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseJanusGraph()
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
