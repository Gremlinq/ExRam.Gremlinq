using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurationBuilderWithUri : IGremlinQueryEnvironmentTransformation
    {
        IGremlinQueryEnvironmentTransformation ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}
