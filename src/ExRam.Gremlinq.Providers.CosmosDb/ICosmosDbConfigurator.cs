using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurator : IProviderConfigurator<ICosmosDbConfigurator>
    {
        ICosmosDbConfigurator At(Uri uri, string databaseName, string graphName);

        ICosmosDbConfigurator AuthenticateBy(string authKey);
    }
}
