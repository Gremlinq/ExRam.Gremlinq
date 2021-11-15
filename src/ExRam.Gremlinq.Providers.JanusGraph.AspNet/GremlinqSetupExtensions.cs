using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.JanusGraph;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup, Func<IJanusGraphConfigurator, IConfiguration, IJanusGraphConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseProvider<IJanusGraphConfigurator>(
                    "JanusGraph",
                    (source, configuratorTransformation) => source.UseJanusGraph(configuratorTransformation));

                    //TODO
                    //(configurator, _) => extraConfiguration?.Invoke(configurator, _) ?? configurator);
        }

        public static GremlinqSetup UseJanusGraph<TVertex, TEdge>(this GremlinqSetup setup, Func<IJanusGraphConfigurator, IConfiguration, IJanusGraphConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseJanusGraph(extraConfiguration)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
