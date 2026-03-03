using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.JanusGraph.AspNet
{
    /// <summary>Extension methods for configuring JanusGraph on <see cref="IGremlinqServicesBuilder"/>.</summary>
    public static class GremlinqServicesBuilderExtensions
    {
        /// <summary>Configures the services builder to use JanusGraph with the given vertex and edge base types.</summary>
        /// <typeparam name="TVertexBase">The base type for vertices.</typeparam>
        /// <typeparam name="TEdgeBase">The base type for edges.</typeparam>
        /// <param name="setup">The services builder.</param>
        public static IGremlinqServicesBuilder<IJanusGraphConfigurator> UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            ArgumentNullException.ThrowIfNull(setup);

            return setup
                .ConfigureBase()
                .UseProvider<IJanusGraphConfigurator>(source => source
                    .UseJanusGraph<TVertexBase, TEdgeBase>)
                .Configure((configurator, section) =>
                {
                    var providerSection = section
                        .GetSection("JanusGraph");

                    return configurator
                        .ConfigureWebSocket(providerSection)
                        .ConfigureBasicAuthentication(providerSection);
                });
        }
    }
}
