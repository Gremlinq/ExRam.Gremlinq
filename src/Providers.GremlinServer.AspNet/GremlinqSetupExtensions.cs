using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup, Func<IGremlinServerConfigurator, IConfigurationSection, IGremlinServerConfigurator>? configuration = null)
        {
            return setup
                .UseProvider<IGremlinServerConfigurator>(
                    "GremlinServer",
                    (source, section) => source
                        .UseGremlinServer<TVertex, TEdge>(configurator =>
                        {
                            configurator = configurator
                                .ConfigureBase(section)
                                .ConfigureWebSocket(section);

                            return configuration?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
