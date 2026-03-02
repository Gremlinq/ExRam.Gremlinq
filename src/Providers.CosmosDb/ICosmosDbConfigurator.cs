using System.Linq.Expressions;

using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    /// <summary>
    /// A configurator for Azure CosmosDb Gremlin connections.
    /// </summary>
    /// <typeparam name="TVertexBase">The base type for all vertex entities in the model.</typeparam>
    public interface ICosmosDbConfigurator<TVertexBase> : IProviderConfigurator<ICosmosDbConfigurator<TVertexBase>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
    {
        /// <summary>
        /// Configures the partition key property for vertex entities.
        /// </summary>
        /// <param name="partitionKeyExpression">An expression selecting the partition key property on the vertex base type.</param>
        ICosmosDbConfigurator<TVertexBase> WithPartitionKey(Expression<Func<TVertexBase, object>> partitionKeyExpression);

        /// <summary>
        /// Sets the CosmosDb database name.
        /// </summary>
        /// <param name="databaseName">The name of the CosmosDb database.</param>
        ICosmosDbConfigurator<TVertexBase> OnDatabase(string databaseName);

        /// <summary>
        /// Sets the CosmosDb graph name.
        /// </summary>
        /// <param name="graphName">The name of the graph within the database.</param>
        ICosmosDbConfigurator<TVertexBase> OnGraph(string graphName);

        /// <summary>
        /// Sets the CosmosDb authentication key.
        /// </summary>
        /// <param name="authKey">The authentication key for the CosmosDb account.</param>
        ICosmosDbConfigurator<TVertexBase> AuthenticateBy(string authKey);
    }
}
