using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.CosmosDb;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup, Action<ProviderSetup<ICosmosDbConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "CosmosDb",
                    (source, configuratorTransformation) => source
                        .UseCosmosDb(configuratorTransformation),
                    setup => setup
                        .ConfigureWebSocket()
                        .Configure((configurator, gremlinqSection, providerSection) =>
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

        public static GremlinqSetup UseCosmosDb<TVertex, TEdge>(this GremlinqSetup setup, Expression<Func<TVertex, object>> partitionKeyExpression, Action<ProviderSetup<ICosmosDbConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseCosmosDb(extraSetupAction)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())
                        .ConfigureProperties(model => model
                            .ConfigureElement<TVertex>(conf => conf
                                .IgnoreOnUpdate(partitionKeyExpression)))));
        }
    }
}
