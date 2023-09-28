using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.CosmosDb;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseCosmosDb<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<ICosmosDbConfigurator<TVertexBase>, IConfigurationSection, ICosmosDbConfigurator<TVertexBase>>? configuratorTransformation = null)
        {
            return setup
                .UseProvider<ICosmosDbConfigurator<TVertexBase>>(
                    "CosmosDb",
                    (source, section) => source
                        .UseCosmosDb<TVertexBase, TEdgeBase>(configurator =>
                        {
                            configurator = configurator
                                .ConfigureBase(section)
                                .ConfigureWebSocket(section);

                            if (section["Database"] is { } databaseName)
                                configurator = configurator.OnDatabase(databaseName);

                            if (section["Graph"] is { } graphName)
                                configurator = configurator.OnGraph(graphName);

                            if (section["AuthKey"] is { } authKey)
                                configurator = configurator.AuthenticateBy(authKey);

                            if (section["PartitionKey"] is { Length: > 0 } partitionKey)
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

                            return configuratorTransformation?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
