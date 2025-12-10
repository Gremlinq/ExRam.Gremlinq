using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Support.TestContainers
{
    public static class ProviderConfiguratorExtensions
    {
        public static IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> UseTestContainers<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> => configurator
                .ConfigureClientFactory(factory => continuation(new TestContainersConfigurator(factory)));
    }
}
