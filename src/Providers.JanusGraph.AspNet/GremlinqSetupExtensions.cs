using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.JanusGraph;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ProviderSetup<IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<IJanusGraphConfigurator, IProviderConfigurationSection, IJanusGraphConfigurator>? configuratorTransformation = null)
        {
            return setup
                .UseProvider<IJanusGraphConfigurator>(
                    "JanusGraph",
                    (source, section) => source
                        .UseJanusGraph<TVertexBase, TEdgeBase>(configurator =>
                        {
                            configurator = configurator
                                .ConfigureBase(section.GremlinqSection)
                                .ConfigureWebSocket(section);

                            return configuratorTransformation?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
