using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static IGremlinQueryEnvironment UseJanusGraph(this IGremlinQueryEnvironment environment,
            Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> builderAction)
        {
            return environment
                .UseGremlinServer(builderAction);
        }
    }
}
