using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public interface ICosmosDbConfigurationBuilder
    {
        ICosmosDbConfigurationBuilderWithUri At(Uri uri, string databaseName, string graphName);
        ICosmosDbConfigurationBuilder ConfigureWebSocket(Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> transformation);
    }
}
