using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.CosmosDb;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<ICosmosDbConfigurator<TVertexBase>> UseCosmosDb<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .UseProvider<ICosmosDbConfigurator<TVertexBase>>(source => source
                    .UseCosmosDb<TVertexBase, TEdgeBase>)
                .ConfigureBase()
                .ConfigureWebSocket()
                .Configure((configurator, gremlinqSection) =>
                {
                    var providerSection = gremlinqSection
                        .GetSection("CosmosDb");

                    if (providerSection["Database"] is { } databaseName)
                        configurator = configurator.OnDatabase(databaseName);

                    if (providerSection["Graph"] is { } graphName)
                        configurator = configurator.OnGraph(graphName);

                    if (providerSection["AuthKey"] is { } authKey)
                        configurator = configurator.AuthenticateBy(authKey);

                    if (providerSection["PartitionKey"] is { Length: > 0 } partitionKey)
                    {
                        if (typeof(TVertexBase).GetProperty(partitionKey, BindingFlags.Instance | BindingFlags.Public) is { GetMethod: { } partitionKeyGetter })
                        {
                            var parameterExpression = Expression.Parameter(typeof(TVertexBase));

                            var partitionKeyExpression = Expression.Lambda<Func<TVertexBase, object>>(
                                Expression.Convert(
                                    Expression.Property(parameterExpression, partitionKeyGetter),
                                    typeof(object)),
                                parameterExpression);

                            configurator = configurator.WithPartitionKey(partitionKeyExpression);
                        }
                    }

                    return configurator;
                });
        }
    }
}
