using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Action<ProviderSetup<IJanusGraphConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "JanusGraph",
                    (source, configuratorTransformation) => source
                        .UseJanusGraph<TVertexBase, TEdgeBase>(configuratorTransformation),
                    setup => setup
                        .Configure()
                        .ConfigureWebSocket(),
                    extraSetupAction);
        }
    }
}
