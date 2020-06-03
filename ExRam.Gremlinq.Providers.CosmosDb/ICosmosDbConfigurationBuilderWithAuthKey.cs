using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public interface ICosmosDbConfigurationBuilderWithAuthKey : IGremlinQueryEnvironmentBuilder
    {
        IGremlinQueryEnvironmentBuilder ConfigureWebSocket(Func<IWebSocketGremlinQueryEnvironmentBuilder, IWebSocketGremlinQueryEnvironmentBuilder> transformation);
    }
}
