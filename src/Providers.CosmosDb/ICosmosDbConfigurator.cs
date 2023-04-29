using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurator : IWebSocketProviderConfigurator<ICosmosDbConfigurator>
    {
        ICosmosDbConfigurator OnDatabase(string databaseName);

        ICosmosDbConfigurator OnGraph(string graphName);

        ICosmosDbConfigurator AuthenticateBy(string authKey);
    }
}
