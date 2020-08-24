using System;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurationBuilder
    {
        ICosmosDbConfigurationBuilderWithUri At(Uri uri, string databaseName, string graphName);
    }
}
