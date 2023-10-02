using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .UseProvider<IGremlinServerConfigurator>(source => source
                    .UseGremlinServer<TVertex, TEdge>)
                .FromProviderSection("GremlinServer")
                .ConfigureBase()
                .ConfigureWebSocket();
        }
    }
}
