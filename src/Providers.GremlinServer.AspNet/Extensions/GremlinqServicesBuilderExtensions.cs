using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Providers.GremlinServer.AspNet.Extensions;

namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet.Extensions
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
                        .ConfigureWebSocket(providerSection);
                });
        }
    }
}
