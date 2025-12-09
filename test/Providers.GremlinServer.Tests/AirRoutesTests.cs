using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Testing.AirRoutes;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using FluentAssertions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class AirRoutesTests : IClassFixture<AirRoutesTests.Fixture>
    {
        public class Fixture : DockerfileTestContainerFixture
        {
            public Fixture() : base("StringIdGremlinServerDockerfile")
            {
            }

            protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
                .UseGremlinServer<Vertex, Edge>(_ => _
                    .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                    .UseNewtonsoftJson());
        }

        private readonly IGremlinQuerySource _source;

        public AirRoutesTests(Fixture fixture)
        {
            _source = fixture
                .GetQuerySource();
        }

        [Fact]
        public async Task CreateSmall()
        {
            await _source
                .CreateAirRoutesSmall(TestContext.Current.CancellationToken);

            await Verify( _source.V().Count().FirstAsync(TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task Idempotency()
        {
            await _source
                .CreateAirRoutesSmall(TestContext.Current.CancellationToken);

            var first = await _source
                .V()
                .Count()
                .FirstAsync(TestContext.Current.CancellationToken);

            await _source
                .CreateAirRoutesSmall(TestContext.Current.CancellationToken);

            var second = await _source
                .V()
                .Count()
                .FirstAsync(TestContext.Current.CancellationToken);

            second
                .Should()
                .Be(first);
        }
    }
}
