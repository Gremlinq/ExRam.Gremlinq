using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;

namespace ExRam.Gremlinq.Support.TestContainers.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class ContainerTeardownTests
    {
        [Fact]
        public async Task Disposing_the_executor_tears_down_the_container()
        {
            IContainer? startedContainer = null;

            var g = GremlinQuerySource.g
                .UseGremlinServer<Vertex, Edge>(configurator => configurator
                    .UseTestContainers(c => c
                        .ConfigureContainer(builder => builder
                            .WithImage("ghcr.io/gremlinq/gremlin-server-mod:3")
                            .WithPortBinding(8182, true)
                            .WithWaitStrategy(Wait
                                .ForUnixContainer()
                                .UntilInternalTcpPortIsAvailable(8182)))
                        .ConfigureClientFactory((poolFactory, container) =>
                        {
                            startedContainer = container;

                            return poolFactory
                                .ConfigureBaseFactory(webSocketFactory => webSocketFactory
                                    .ConfigureUri(_ => new Uri($"ws://localhost:{container.GetMappedPublicPort(8182)}")));
                        }))
                    .UseNewtonsoftJson());

            var result = await g
                .Inject(1, 2, 3)
                .Sum()
                .FirstAsync(TestContext.Current.CancellationToken);

            result
                .Should()
                .Be(6);

            startedContainer
                .Should()
                .NotBeNull();

            startedContainer!
                .State
                .Should()
                .Be(TestcontainersStates.Running);

            var executor = g
                .AsAdmin()
                .Environment
                .Executor;

            executor
                .Should()
                .BeAssignableTo<IAsyncDisposable>();

            await ((IAsyncDisposable)executor).DisposeAsync();

            await Task.Delay(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
            
            startedContainer
                .State
                .Should()
                .NotBe(TestcontainersStates.Running);
        }
    }
}