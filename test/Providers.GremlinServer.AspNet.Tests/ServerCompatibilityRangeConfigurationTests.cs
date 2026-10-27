using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet.Tests
{
    public class ServerCompatibilityRangeConfigurationTests
    {
        [Fact]
        public void Without_range() => GetServerCompatibilityRange([])
            .Should()
            .Be(GremlinQuerySource.g.AsAdmin().Environment.ServerCompatibilityRange);

        [Fact]
        public void Lowest_and_highest() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.7.7" },
                { "Gremlinq:ServerCompatibilityRange:Highest", "3.8.2" }
            })
            .Should()
            .Be(VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2));

        [Fact]
        public void Lowest_only() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.7.7" }
            })
            .Should()
            .Be(VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_7_7));

        [Fact]
        public void Highest_only() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Highest", "3.8.2" }
            })
            .Should()
            .Be(VersionRange.From(TinkerPopVersions.V3_7_1, TinkerPopVersions.V3_8_2));

        [Fact]
        public void Empty_values() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Lowest", "" },
                { "Gremlinq:ServerCompatibilityRange:Highest", "" }
            })
            .Should()
            .Be(GremlinQuerySource.g.AsAdmin().Environment.ServerCompatibilityRange);

        [Fact]
        public void Unreadable_version_throws() => FluentActions
            .Invoking(() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.8.x" }
            }))
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Gremlinq:ServerCompatibilityRange:Lowest*");

        [Fact]
        public void Lowest_above_highest_throws() => FluentActions
            .Invoking(() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.8.0" }
            }))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        [Fact]
        public void From_base_section() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "MyApp:Gremlinq:ServerCompatibilityRange:Lowest", "3.8.2" },
                    { "MyApp:Gremlinq:ServerCompatibilityRange:Highest", "3.8.2" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .FromBaseSection("MyApp")
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .ServerCompatibilityRange
            .Should()
            .Be(VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2));

        private static VersionRange GetServerCompatibilityRange(Dictionary<string, string?> configuration) => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(configuration)
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .ServerCompatibilityRange;
    }
}
