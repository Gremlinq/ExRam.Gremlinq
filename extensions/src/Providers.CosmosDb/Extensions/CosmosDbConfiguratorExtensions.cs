using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    /// <summary>
    /// Provides extension methods for <see cref="ICosmosDbConfigurator{TVertexBase}"/>.
    /// </summary>
    public static class CosmosDbConfiguratorExtensions
    {
        /// <summary>
        /// Configures the CosmosDb connection URI, database name, and graph name.
        /// </summary>
        /// <typeparam name="TVertexBase">The base type for all vertex entities.</typeparam>
        /// <param name="configurator">The configurator to configure.</param>
        /// <param name="uri">The URI string of the CosmosDb Gremlin endpoint.</param>
        /// <param name="databaseName">The name of the CosmosDb database.</param>
        /// <param name="graphName">The name of the graph within the database.</param>
        public static ICosmosDbConfigurator<TVertexBase> At<TVertexBase>(this ICosmosDbConfigurator<TVertexBase> configurator, string uri, string databaseName, string graphName)
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(uri);
            ArgumentNullException.ThrowIfNull(databaseName);
            ArgumentNullException.ThrowIfNull(graphName);

            return configurator
                .At(new Uri(uri), databaseName, graphName);
        }

        /// <summary>
        /// Configures the CosmosDb connection URI, database name, and graph name.
        /// </summary>
        /// <typeparam name="TVertexBase">The base type for all vertex entities.</typeparam>
        /// <param name="configurator">The configurator to configure.</param>
        /// <param name="uri">The URI of the CosmosDb Gremlin endpoint.</param>
        /// <param name="databaseName">The name of the CosmosDb database.</param>
        /// <param name="graphName">The name of the graph within the database.</param>
        public static ICosmosDbConfigurator<TVertexBase> At<TVertexBase>(this ICosmosDbConfigurator<TVertexBase> configurator, Uri uri, string databaseName, string graphName)
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(uri);
            ArgumentNullException.ThrowIfNull(databaseName);
            ArgumentNullException.ThrowIfNull(graphName);

            return configurator
                .At(uri)
                .OnDatabase(databaseName)
                .OnGraph(graphName);
        }

        /// <summary>
        /// Configures the CosmosDb connection to use the default localhost URI (<c>ws://localhost:8182</c>) with the specified database and graph names.
        /// </summary>
        /// <typeparam name="TVertexBase">The base type for all vertex entities.</typeparam>
        /// <param name="configurator">The configurator to configure.</param>
        /// <param name="databaseName">The name of the CosmosDb database.</param>
        /// <param name="graphName">The name of the graph within the database.</param>
        public static ICosmosDbConfigurator<TVertexBase> AtLocalhost<TVertexBase>(this ICosmosDbConfigurator<TVertexBase> configurator, string databaseName, string graphName)
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(databaseName);
            ArgumentNullException.ThrowIfNull(graphName);

            return configurator.At("ws://localhost:8182", databaseName, graphName);
        }
    }
}
