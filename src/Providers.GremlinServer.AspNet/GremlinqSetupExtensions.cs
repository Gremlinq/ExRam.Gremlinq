using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ProviderSetup<IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup, Func<IGremlinServerConfigurator, IProviderConfigurationSection, IGremlinServerConfigurator>? configuratorTransformation = null)
        {
            return setup
                .UseProvider<IGremlinServerConfigurator>(
                    "GremlinServer",
                    (source, section) => source
                        .UseGremlinServer<TVertex, TEdge>(configurator =>
                        {
                            configurator = configurator
                                .ConfigureBase(section.GremlinqSection)
                                .ConfigureWebSocket(section);

                            return configuratorTransformation?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
