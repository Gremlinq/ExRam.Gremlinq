using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GremlinServerFixture : GremlinqFixture
    {
        private readonly IContainer _gremlinServerContainer;

        public GremlinServerFixture() : this(new ContainerBuilder()
            .WithImage("tinkerpop/gremlin-server:3.7.0")
            .WithPortBinding(8182)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(8182))
            .Build())
        {
        }

        private GremlinServerFixture(IContainer container) : base(() => GetQuerySource(container))
        {
            _gremlinServerContainer = container;
        }

        private static async Task<IGremlinQuerySource> GetQuerySource(IContainer container) => g
            .UseGremlinServer(_ => _
                .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                .UseNewtonsoftJson());

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await _gremlinServerContainer.StartAsync();
        }

        public override async Task DisposeAsync()
        {
            await _gremlinServerContainer.StopAsync();

            await base.DisposeAsync();
        }
    }
}
