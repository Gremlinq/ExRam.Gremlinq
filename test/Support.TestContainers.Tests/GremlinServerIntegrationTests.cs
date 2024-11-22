using DotNet.Testcontainers.Builders;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Support.TestContainers.Tests
{
    public class GremlinServerIntegrationTests : VerifyBase
    {
        private readonly IGremlinQuerySource _g;

        public GremlinServerIntegrationTests() : base()
        {
            _g = GremlinQuerySource.g
                .UseGremlinServer<Vertex, Edge>(configurator => configurator
                    .ConfigureClientFactory(factory => factory
                        .UseTestContainers(c => c
                            .ConfigureContainer(builder => builder
                                .WithImage("tinkerpop/gremlin-server")
                                .WithPortBinding(8182, true)
                                .WithWaitStrategy(Wait
                                    .ForUnixContainer()
                                    .UntilPortIsAvailable(8182)))
                            .ConfigureClientFactory((poolFactory, container) => poolFactory
                                    .ConfigureBaseFactory(webSocketFactory => webSocketFactory
                                        .ConfigureUri(_ => new Uri($"ws://localhost:{container.GetMappedPublicPort(8182)}"))))))
                    .UseNewtonsoftJson());
        }

        [Fact]
        public Task Inject_sum() => Verify(_g
            .Inject(1, 2, 3)
            .Sum()
            .FirstAsync());
    }
}
