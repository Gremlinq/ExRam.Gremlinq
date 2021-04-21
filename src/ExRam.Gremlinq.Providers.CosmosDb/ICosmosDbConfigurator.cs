using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurator : IProviderConfigurator<ICosmosDbConfigurator>
    {
        ICosmosDbConfigurator OnDatabase(string databaseName);

        ICosmosDbConfigurator OnGraph(string graphName);

        ICosmosDbConfigurator AuthenticateBy(string authKey);
    }
}
