using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public interface ICosmosDbConfigurationBuilderWithAuthKey : IGremlinQueryEnvironmentBuilder
    {
        ICosmosDbConfigurationBuilderWithAuthKey ConfigureWebSocket(Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> transformation);
    }
}
