using System;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketRemoteConfigurator
    {
        IWebSocketRemoteConfigurator WithClientFactory(Func<IGremlinClient> clientFactory);

        IWebSocketRemoteConfigurator WithSerializerFactory(IGraphsonDeserializerFactory deserializer);

        IGremlinQueryExecutor Build();
    }
}
