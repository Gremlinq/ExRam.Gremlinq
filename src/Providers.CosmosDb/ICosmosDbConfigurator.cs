using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurator<TVertexBase> : IWebSocketProviderConfigurator<ICosmosDbConfigurator<TVertexBase>>
    {
        ICosmosDbConfigurator<TVertexBase> OnDatabase(string databaseName);

        ICosmosDbConfigurator<TVertexBase> OnGraph(string graphName);

        ICosmosDbConfigurator<TVertexBase> AuthenticateBy(string authKey);
    }
}
