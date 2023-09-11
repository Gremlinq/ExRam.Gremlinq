using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class TestContainerFixture : GremlinqFixture, IAsyncLifetime
    {
        private readonly IContainer _container;

        protected TestContainerFixture(string image, int port, Func<IContainer, IGremlinQuerySource> sourceFactory) : this(
            new ContainerBuilder()
                .WithImage(image)
                .WithPortBinding(port)
                .WithWaitStrategy(Wait
                    .ForUnixContainer()
                    .UntilPortIsAvailable(port))
                .Build(),
            sourceFactory)
        {
        }

        private TestContainerFixture(IContainer container, Func<IContainer, IGremlinQuerySource> sourceFactory) : base(async () => sourceFactory(container))
        {
            _container = container;
        }

        public async Task InitializeAsync() => await _container.StartAsync();

        public async Task DisposeAsync() => await _container.StopAsync();
    }
}
