using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static IGremlinqProviderSetup<IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this IGremlinqSetup setup)
        {
            return setup
                .UseProvider<IGremlinServerConfigurator>(
                    "GremlinServer",
                    (source, configurationContinuation) => source.UseGremlinServer<TVertex, TEdge>(configurationContinuation))
                .Configure((configurator, section) => configurator
                        .ConfigureBase(section.GremlinqSection)
                        .ConfigureWebSocket(section));
        }
    }
}
