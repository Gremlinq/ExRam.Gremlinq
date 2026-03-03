using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet
{
    /// <summary>
    /// Provides extension methods for <see cref="IGremlinqServicesBuilder"/> to register the Gremlin Server provider with ASP.NET Core dependency injection.
    /// </summary>
    public static class GremlinqServicesBuilderExtensions
    {
        /// <summary>
        /// Registers the Apache TinkerPop Gremlin Server provider and configures it from the application's configuration section.
        /// </summary>
        /// <typeparam name="TVertex">The base type for all vertex entities.</typeparam>
        /// <typeparam name="TEdge">The base type for all edge entities.</typeparam>
        /// <param name="setup">The services builder to configure.</param>
        public static IGremlinqServicesBuilder<IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this IGremlinqServicesBuilder setup)
        {
            ArgumentNullException.ThrowIfNull(setup);

            return setup
                .ConfigureBase()
                .UseProvider<IGremlinServerConfigurator>(source => source
                    .UseGremlinServer<TVertex, TEdge>)
                .Configure((configurator, section) =>
                {
                    var providerSection = section
                        .GetSection("GremlinServer");

                    return configurator
                        .ConfigureWebSocket(providerSection)
                        .ConfigureBasicAuthentication(providerSection);
                });
        }
    }
}
