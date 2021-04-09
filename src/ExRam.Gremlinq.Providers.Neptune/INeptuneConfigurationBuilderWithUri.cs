using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurationBuilderWithUri : IGremlinQueryExecutorBuilder
    {
        IGremlinQueryExecutorBuilder ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}
