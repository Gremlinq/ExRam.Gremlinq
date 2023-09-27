using System.Linq.Expressions;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.CosmosDb;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseCosmosDb<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Expression<Func<TVertexBase, object>> partitionKeyExpression, Action<ProviderSetup<ICosmosDbConfigurator<TVertexBase>>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "CosmosDb",
                    (source, configuratorTransformation) => source
                        .UseCosmosDb<TVertexBase, TEdgeBase>(partitionKeyExpression, configuratorTransformation),
                    setup => setup
                        .Configure()
                        .ConfigureWebSocket()
                        .Configure((configurator, providerSection) =>
                        {
                            if (providerSection["Database"] is { } databaseName)
                                configurator = configurator.OnDatabase(databaseName);

                            if (providerSection["Graph"] is { } graphName)
                                configurator = configurator.OnGraph(graphName);

                            if (providerSection["AuthKey"] is { } authKey)
                                configurator = configurator.AuthenticateBy(authKey);

                            return configurator;
                        }),
                    extraSetupAction);
        }
    }
}
