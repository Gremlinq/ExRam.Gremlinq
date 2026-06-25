using DotNet.Testcontainers.Builders;

using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Support.TestContainers
{
    /// <summary>Extension methods for configuring Testcontainers on provider configurators.</summary>
    public static class ProviderConfiguratorExtensions
    {
        /// <summary>Configures the provider to use Testcontainers for the graph database.</summary>
        /// <typeparam name="TConfigurator">The configurator type.</typeparam>
        /// <param name="configurator">The provider configurator.</param>
        /// <param name="continuation">The Testcontainers configuration continuation.</param>
        public static TConfigurator UseTestContainers<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Func<TestContainersConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> continuation)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(continuation);

            return configurator
                .ConfigureClientFactory(factory => continuation(new TestContainersConfigurator(factory)));
        }

        /// <summary>Configures the provider to run a Gremlin Server container.</summary>
        /// <typeparam name="TConfigurator">The configurator type.</typeparam>
        /// <param name="configurator">The provider configurator.</param>
        /// <param name="tag">The Docker image tag.</param>
        public static TConfigurator UseGremlinServerContainer<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, string tag = "latest")
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(tag);

            return configurator
                .UseTestContainersWithDefaultSetup("tinkerpop/gremlin-server", tag);
        }

        /// <summary>
        ///  Runs a container from the 'ghcr.io/gremlinq/gremlin-server-mod' image upon GremlinClient creation.
        ///  See https://github.com/Gremlinq/Gremlinq.Dockerfiles.GremlinServerMod
        /// </summary>
        /// <typeparam name="TConfigurator"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static TConfigurator UseGremlinServerModContainer<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, string tag = "3")
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(tag);

            return configurator
                .UseTestContainersWithDefaultSetup("ghcr.io/gremlinq/gremlin-server-mod", tag);
        }

        /// <summary>Configures the provider to run a JanusGraph container.</summary>
        /// <typeparam name="TConfigurator">The configurator type.</typeparam>
        /// <param name="configurator">The provider configurator.</param>
        /// <param name="tag">The Docker image tag.</param>
        public static TConfigurator UseJanusGraphContainer<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, string tag = "latest")
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(tag);

            return configurator
                .UseTestContainersWithDefaultSetup("janusgraph/janusgraph", tag);
        }

        private static TConfigurator UseTestContainersWithDefaultSetup<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, string image, string tag)
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
