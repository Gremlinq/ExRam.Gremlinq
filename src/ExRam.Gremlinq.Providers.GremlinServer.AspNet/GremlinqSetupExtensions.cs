using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.GremlinServer;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup, Func<IGremlinServerConfigurator, IProviderConfiguration, IGremlinServerConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseProvider<IGremlinServerConfigurator>(
                    "GremlinServer",
                    (source, configuratorTransformation) => source.UseGremlinServer(configuratorTransformation));

                    //TODO
                    //(configurator, _) => extraConfiguration?.Invoke(configurator, _) ?? configurator);
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
