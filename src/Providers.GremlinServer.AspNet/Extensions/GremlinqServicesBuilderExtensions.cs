using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .ConfigureBase()
                .UseProvider<IGremlinServerConfigurator>(source => source
                    .UseGremlinServer<TVertex, TEdge>)
                .Configure((configurator, section) =>
                {
                    var providerSection = section
                        .GetSection("GremlinServer");

                    return configurator
                        .ConfigureWebSocket<IGremlinServerConfigurator, IGremlinqClientFactory>(providerSection)
                        .ConfigureBasicAuthentication<IGremlinServerConfigurator, IGremlinqClientFactory>(providerSection);
                });
        }
    }
}
