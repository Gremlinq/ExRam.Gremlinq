using Docker.DotNet;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class TestContainerFixtureBase : GremlinqFixture
    {
        private readonly int _port;

        private IContainer? _container;

        protected TestContainerFixtureBase(int port = 8182)
        {
            _port = port;
        }

        protected sealed override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g)
        {
            if (_container is { } container)
                return TransformQuerySource(container, g);

            throw new InvalidOperationException();
        }

        public override async ValueTask InitializeAsync()
        {
            for(var i = 0; i < 42; i++)
            {
                try
                {
		            var containerBuilder = new ContainerBuilder(await GetImage())
		                .WithName(Guid.NewGuid().ToString("N"))
		                .WithPortBinding(_port, true)
		                .WithAutoRemove(true)
		                .WithWaitStrategy(Wait
		                    .ForUnixContainer()
		                    .UntilInternalTcpPortIsAvailable(_port))
		                .WithReuse(false);

		            _container = CustomizeContainer(containerBuilder)
		                .Build();

		            await _container
		                .StartAsync();

                    break;
                }
                catch (DockerApiException)
                {
                    await Task.Delay(500);
                }
                catch (InvalidOperationException)
                {
                    await Task.Delay(500);
                }
            }

            await base.InitializeAsync();
        }

        public override async ValueTask DisposeAsync()
        {
            if (_container is { } container)
            {
                try
                {
                    await using (container)
                    {
                        await container.StopAsync();
                    }
                }
                catch (DockerContainerNotFoundException)
                {

                }
            }

            await base.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        protected abstract Task<IImage> GetImage(); 

        protected virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder) => builder;

        protected abstract IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g);
    }
}
