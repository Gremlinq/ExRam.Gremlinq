using System;
using ExRam.Gremlinq.Providers.Neptune;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseNeptune(this GremlinqSetup setup)
        {
            return setup
                .UseProvider<INeptuneConfigurator>(
                    "Neptune",
                    (e, f) => e.UseNeptune(f),
                    (configurator, configuration) => (configuration["ElasticSearchEndPoint"] is { } endPoint)
                        ? configurator.UseElasticSearch(new Uri(endPoint))
                        : configurator);
        }

        public static GremlinqSetup UseNeptune<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseNeptune()
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
