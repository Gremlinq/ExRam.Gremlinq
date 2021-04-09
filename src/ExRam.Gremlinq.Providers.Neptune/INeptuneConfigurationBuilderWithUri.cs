using System;

using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public interface INeptuneConfigurationBuilderWithUri : IGremlinQueryExecutorBuilder
    {
        IGremlinQueryExecutorBuilder ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}
