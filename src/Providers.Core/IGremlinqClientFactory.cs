using System.Net.WebSockets;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IPoolGremlinqClientFactory<TBaseFactory> : IGremlinqClientFactory
        where TBaseFactory : IGremlinqClientFactory
    {
        IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(Func<TBaseFactory, TBaseFactory> transformation);

        IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize);

        IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection);
    }

    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory
    {
        IWebSocketGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation);

        IWebSocketGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration);
    }

    public interface IGremlinqClientFactory
    {
        IGremlinqClient Create(IGremlinQueryEnvironment environment);
    }
}
