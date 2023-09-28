using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<IJanusGraphConfigurator, IConfigurationSection, IJanusGraphConfigurator>? configuratorTransformation = null)
        {
            return setup
                .UseProvider<IJanusGraphConfigurator>(
                    "JanusGraph",
                    (source, section) => source
                        .UseJanusGraph<TVertexBase, TEdgeBase>(configurator =>
                        {
                            configurator = configurator
                                .ConfigureBase(section)
                                .ConfigureWebSocket(section);

                            return configuratorTransformation?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
