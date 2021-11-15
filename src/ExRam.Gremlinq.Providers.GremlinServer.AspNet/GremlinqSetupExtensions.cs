using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup, Action<ProviderSetup<IGremlinServerConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "GremlinServer",
                    (source, configuratorTransformation) => source
                        .UseGremlinServer(configuratorTransformation),
                    setup => { },
                    extraSetupAction);
        }

        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<IGremlinServerConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseGremlinServer(extraSetupAction)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
