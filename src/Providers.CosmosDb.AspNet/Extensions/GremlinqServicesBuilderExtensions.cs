using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Providers.CosmosDb.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<ICosmosDbConfigurator<TVertexBase>> UseCosmosDb<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
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
