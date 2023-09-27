using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<IGremlinServerConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "GremlinServer",
                    (source, configuratorTransformation) => source
                        .UseGremlinServer<TVertex, TEdge>(configuratorTransformation),
                    setup => setup
                        .Configure()
                        .ConfigureWebSocket(),
                    extraSetupAction);
        }
    }
}
