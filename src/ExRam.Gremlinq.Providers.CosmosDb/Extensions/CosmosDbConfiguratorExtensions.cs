using System;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfiguratorExtensions
    {
        public static ICosmosDbConfigurator At(this ICosmosDbConfigurator configurator, string uri, string databaseName, string graphName)
        {
            return configurator.At(new Uri(uri), databaseName, graphName);
        }

        public static ICosmosDbConfigurator AtLocalhost(this ICosmosDbConfigurator configurator, string uri, string databaseName, string graphName)
        {
            return configurator.At("ws://localhost:8182", databaseName, graphName);
        }
    }
}
