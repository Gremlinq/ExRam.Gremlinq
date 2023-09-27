using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.CosmosDb;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseCosmosDb<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<ICosmosDbConfigurator<TVertexBase>, IConfigurationSection, ICosmosDbConfigurator<TVertexBase>>? configuration = null)
        {
            return setup
                .UseProvider<ICosmosDbConfigurator<TVertexBase>>(
                    "CosmosDb",
                    (source, configuratorTransformation) => source
                        .UseCosmosDb<TVertexBase, TEdgeBase>(configuratorTransformation),
                    setup => setup
                        .Configure((configurator, providerSection) =>
                        {
                            configurator = configurator
                                .ConfigureBase(providerSection)
                                .ConfigureWebSocket(providerSection);

                            if (providerSection["Database"] is { } databaseName)
                                configurator = configurator.OnDatabase(databaseName);

                            if (providerSection["Graph"] is { } graphName)
                                configurator = configurator.OnGraph(graphName);

                            if (providerSection["AuthKey"] is { } authKey)
                                configurator = configurator.AuthenticateBy(authKey);

                            return configuration?.Invoke(configurator, providerSection) ?? configurator;
                        }));
        }
    }
}
