using System;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurator
    {
        ICosmosDbConfiguratorWithUri At(Uri uri, string databaseName, string graphName);
    }
}
