using DotNet.Testcontainers.Builders;

using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Support.TestContainers
{
    /// <summary>Configurator for setting up Testcontainers-based graph database containers.</summary>
    public readonly struct TestContainersConfigurator
    {
        private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>? _clientFactory;

        internal TestContainersConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>Configures the container builder.</summary>
        /// <param name="containerBuilderTransformation">The container builder transformation.</param>
        public TestContainersWithContainerConfigurator ConfigureContainer(Func<ContainerBuilder, ContainerBuilder> containerBuilderTransformation)
        {
            ArgumentNullException.ThrowIfNull(containerBuilderTransformation);

            return _clientFactory is { } clientFactory
                ? new TestContainersWithContainerConfigurator(clientFactory, containerBuilderTransformation)
                : throw new InvalidOperationException();
        }
    }
}
