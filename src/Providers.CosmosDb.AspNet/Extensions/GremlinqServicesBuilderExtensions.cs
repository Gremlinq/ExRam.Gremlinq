using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.CosmosDb.AspNet
{
    /// <summary>
    /// Provides extension methods for <see cref="IGremlinqServicesBuilder"/> to register the Azure CosmosDb provider with ASP.NET Core dependency injection.
    /// </summary>
    public static class GremlinqServicesBuilderExtensions
    {
        /// <summary>
        /// Registers the Azure CosmosDb Gremlin provider and configures it from the application's configuration section.
        /// </summary>
        /// <typeparam name="TVertexBase">The base type for all vertex entities.</typeparam>
        /// <typeparam name="TEdgeBase">The base type for all edge entities.</typeparam>
        /// <param name="setup">The services builder to configure.</param>
        [Obsolete(
            """
            This method and the CosmosDb provider packages will be removed in ExRam.Gremlinq v14.

            To keep using the CosmosDb provider packages beyond ExRam.Gremlinq v13, subscribe to the Gremlinq.Extensions CosmosDb-bundle, which seamlessly extends ExRam.Gremlinq with features that move beyond the core project.

            Existing customers of any Gremlinq.Extensions product already have access to all ExRam.Gremlinq.Providers.CosmosDb.* packages on the Gremlinq.Extensions NuGet feed.
            Simply update to the latest 13.x version, and this message will disappear.

            For details on the v14 transition and available options, see https://docs.gremlinq.net/cosmosdb-provider-packages/
            """,
            error: false)]
        public static IGremlinqServicesBuilder<ICosmosDbConfigurator<TVertexBase>> UseCosmosDb<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            ArgumentNullException.ThrowIfNull(setup);

            return setup
                .ConfigureBase()
                .UseProvider<ICosmosDbConfigurator<TVertexBase>>(source => source
                    .UseCosmosDb<TVertexBase, TEdgeBase>)
                .Configure((configurator, gremlinqSection) =>
                {
                var providerSection = gremlinqSection
                    .GetSection("CosmosDb");

                configurator = configurator
                    .ConfigureWebSocket(providerSection);

                if (providerSection["Database"] is { } databaseName)
                    configurator = configurator.OnDatabase(databaseName);

                if (providerSection["Graph"] is { } graphName)
                    configurator = configurator.OnGraph(graphName);

                if (providerSection["AuthKey"] is { } authKey)
                    configurator = configurator.AuthenticateBy(authKey);

                if (providerSection["PartitionKey"] is { Length: > 0 } partitionKey)
                {
                    var maybeElementType = typeof(TVertexBase);

                    while (true)
                    {
                        if (maybeElementType is { } elementType)
                        {
                            if (elementType.GetProperty(partitionKey, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly) is { GetMethod: { } partitionKeyGetter })
                            {
                                var parameterExpression = Expression.Parameter(typeof(TVertexBase));

                                var partitionKeyExpression = Expression.Lambda<Func<TVertexBase, object>>(
                                    Expression.Convert(
                                        Expression.Property(parameterExpression, partitionKeyGetter),
                                        typeof(object)),
                                    parameterExpression);

                                configurator = configurator.WithPartitionKey(partitionKeyExpression);
                                break;
                            }

                            maybeElementType = elementType.BaseType;
                        }
                        else
                            throw new MissingMemberException($"The class {typeof(TVertexBase).Name} does not define a publicly accessible and readable property for the partition key called {partitionKey}.");
                    }
                }

                return configurator;
                });
        }
    }
}
