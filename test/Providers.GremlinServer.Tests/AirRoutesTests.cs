using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Testing.AirRoutes;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using FluentAssertions;
using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class AirRoutesTests : IClassFixture<AirRoutesTests.Fixture>
    {
        internal abstract class Element
        {
            public string? Id { get; set; }
        }

        internal sealed class Airport : Element
        {
            public string? Code { get; set; }
            public string? ICAO { get; set; }
            public string? City { get; set; }
            public string? Region { get; set; }
            public string? Country { get; set; }
            public string? Description { get; set; }

            public int Runways { get; set; }
            public int Elevation { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int LongestRunway { get; set; }
        }

        internal sealed class Route : Element
        {
            public long Distance { get; set; }
        }

        public class Fixture : DockerfileTestContainerFixture
        {
            public Fixture() : base("StringIdGremlinServerDockerfile")
            {
            }

            protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
                .UseGremlinServer<Airport, Route>(_ => _
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

            var airports = await _source
                .V<Airport>();

            await Verify(_source.V().Count().FirstAsync(TestContext.Current.CancellationToken));
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
