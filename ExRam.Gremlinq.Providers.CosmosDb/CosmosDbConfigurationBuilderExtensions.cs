using System;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfigurationBuilderExtensions
    {
        public static ICosmosDbConfigurationBuilderWithUri At(this ICosmosDbConfigurationBuilder builder, string uri, string databaseName, string graphName)
        {
            return builder.At(new Uri(uri), databaseName, graphName);
        }
    }
}
