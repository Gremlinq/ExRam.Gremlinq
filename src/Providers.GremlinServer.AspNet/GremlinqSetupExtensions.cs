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
                    (source, configuratorTransformation) => source
                        .UseGremlinServer<TVertex, TEdge>(configuratorTransformation),
                    setup => setup
                        .Configure()
                        .Configure((configurator, section) =>
                        {
                            configurator = configurator
                                .ConfigureWebSocket(section);

                            return configuration?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
