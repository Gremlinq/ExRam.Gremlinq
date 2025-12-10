using DotNet.Testcontainers.Builders;

using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Support.TestContainers
{
    public static class ProviderConfiguratorExtensions
    {
        public static IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> UseTestContainers<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> => configurator
                .ConfigureClientFactory(factory => continuation(new TestContainersConfigurator(factory)));

        public static IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> UseGremlinServerContainer<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation, string tag = "latest")
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> => configurator
                .UseTestContainersWithDefaultSetup("tinkerpop/gremlin-server", tag);

        public static IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> UseGremlinServerModContainer<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation, string tag = "3")
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> => configurator
                .UseTestContainersWithDefaultSetup("ghcr.io/gremlinq/gremlin-server-mod", tag);

        public static IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> UseJanusGraphContainer<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation, string tag = "latest")
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> => configurator
                .UseTestContainersWithDefaultSetup("janusgraph/janusgraph", tag);

        private static IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> UseTestContainersWithDefaultSetup<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, string image, string tag)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> => configurator
                .UseTestContainers(c => c
                    .ConfigureContainer(builder => builder
                        .WithImage($"{image}:{tag}")
                        .WithPortBinding(8182, true)
                        .WithWaitStrategy(Wait
                            .ForUnixContainer()
                            .UntilInternalTcpPortIsAvailable(8182)))
                    .ConfigureClientFactory((poolFactory, container) => poolFactory
                            .ConfigureBaseFactory(webSocketFactory => webSocketFactory
                                .ConfigureUri(_ => new Uri($"ws://localhost:{container.GetMappedPublicPort(8182)}")))));
    }
}
