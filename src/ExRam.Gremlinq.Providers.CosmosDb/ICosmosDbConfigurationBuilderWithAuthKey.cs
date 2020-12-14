using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurationBuilderWithAuthKey : IGremlinQueryExecutorBuilder
    {
        IGremlinQueryExecutorBuilder ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}
