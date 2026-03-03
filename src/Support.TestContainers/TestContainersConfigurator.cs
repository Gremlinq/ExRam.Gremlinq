using DotNet.Testcontainers.Builders;

using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Support.TestContainers
{
    public readonly struct TestContainersConfigurator
    {
        private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>? _clientFactory;

        internal TestContainersConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public TestContainersWithContainerConfigurator ConfigureContainer(Func<ContainerBuilder, ContainerBuilder> containerBuilderTransformation)
        {
            ArgumentNullException.ThrowIfNull(containerBuilderTransformation);

            return _clientFactory is { } clientFactory
                ? new TestContainersWithContainerConfigurator(clientFactory, containerBuilderTransformation)
                : throw new InvalidOperationException();
        }
    }
}
