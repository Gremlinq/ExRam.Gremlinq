using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public interface ICosmosDbConfigurationBuilderWithAuthKey
    {
        IWebSocketConfigurationBuilder Build();
        ICosmosDbConfigurationBuilderWithAuthKey ConfigureWebSocket(Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> transformation);
    }
}