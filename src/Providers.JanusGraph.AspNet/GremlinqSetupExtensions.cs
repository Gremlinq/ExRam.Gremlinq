using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseJanusGraph<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<IJanusGraphConfigurator, IConfigurationSection, IJanusGraphConfigurator>? configuration = null)
        {
            return setup
                .UseProvider<IJanusGraphConfigurator>(
                    "JanusGraph",
                    (source, configuratorTransformation) => source
                        .UseJanusGraph<TVertexBase, TEdgeBase>(configuratorTransformation),
                    setup => setup
                        .Configure((configurator, section) =>
                        {
                            configurator = configurator
                                .ConfigureBase(section)
                                .ConfigureWebSocket(section);

                            return configuration?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
