using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqProviderServicesBuilder<IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this IGremlinqServicesBuilder setup)
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
