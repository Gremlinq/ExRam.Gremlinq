using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Support.TestContainers
{
    public static class ProviderConfiguratorExtensions
    {
        public static IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> UseTestContainers(this IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> factory, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation) => continuation(new TestContainersConfigurator(factory));
    }
}
