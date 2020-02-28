using System;

namespace ExRam.Gremlinq.Core
{
    public interface ICosmosDbConfigurationBuilder
    {
        ICosmosDbConfigurationBuilderWithUri At(Uri uri, string databaseName, string graphName);
    }
}