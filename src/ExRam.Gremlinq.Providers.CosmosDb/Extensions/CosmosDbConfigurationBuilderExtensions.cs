using System;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfigurationBuilderExtensions
    {
        public static ICosmosDbConfigurator At(this ICosmosDbConfigurator builder, string uri, string databaseName, string graphName)
        {
            return builder.At(new Uri(uri), databaseName, graphName);
        }
    }
}
