using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfiguratorExtensions
    {
        public static ICosmosDbConfigurator<TVertexBase> At<TVertexBase>(this ICosmosDbConfigurator<TVertexBase> configurator, string uri, string databaseName, string graphName)
        {
            return configurator
                .At(new Uri(uri), databaseName, graphName);
        }

        public static ICosmosDbConfigurator<TVertexBase> At<TVertexBase>(this ICosmosDbConfigurator<TVertexBase> configurator, Uri uri, string databaseName, string graphName)
        {
            return configurator
                .At(uri)
                .OnDatabase(databaseName)
                .OnGraph(graphName);
        }

        public static ICosmosDbConfigurator<TVertexBase> AtLocalhost<TVertexBase>(this ICosmosDbConfigurator<TVertexBase> configurator, string databaseName, string graphName)
        {
            return configurator.At("ws://localhost:8182", databaseName, graphName);
        }
    }
}
