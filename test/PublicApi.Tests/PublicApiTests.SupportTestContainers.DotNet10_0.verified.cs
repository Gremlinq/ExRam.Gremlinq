namespace ExRam.Gremlinq.Support.TestContainers
{
    public static class ProviderConfiguratorExtensions
    {
        public static TConfigurator UseGremlinServerContainer<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> configurator, System.Func<ExRam.Gremlinq.Support.TestContainers.TestContainersConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> continuation, string tag = "latest")
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator UseGremlinServerModContainer<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> configurator, System.Func<ExRam.Gremlinq.Support.TestContainers.TestContainersConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> continuation, string tag = "3")
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator UseJanusGraphContainer<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> configurator, System.Func<ExRam.Gremlinq.Support.TestContainers.TestContainersConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> continuation, string tag = "latest")
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator UseTestContainers<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> configurator, System.Func<ExRam.Gremlinq.Support.TestContainers.TestContainersConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> continuation)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
    }
    public readonly struct TestContainersConfigurator
    {
        public ExRam.Gremlinq.Support.TestContainers.TestContainersWithContainerConfigurator ConfigureContainer(System.Func<DotNet.Testcontainers.Builders.ContainerBuilder, DotNet.Testcontainers.Builders.ContainerBuilder> containerBuilderTransformation) { }
    }
    public readonly struct TestContainersWithContainerConfigurator
    {
        public ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory> ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>, DotNet.Testcontainers.Containers.IContainer, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> factoryTransformation) { }
    }
}