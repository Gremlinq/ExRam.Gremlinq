using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurationBuilderWithUri : IGremlinQueryEnvironmentTransformation
    {
        IGremlinQueryEnvironmentTransformation ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}
