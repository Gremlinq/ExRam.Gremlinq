using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.Neptune.AspNet.Tests
{
    public class CompatibilityConfigurationTests
    {
        private static readonly VersionRange Engine14Range = VersionRange.From(TinkerPopVersions.V3_7_1, TinkerPopVersions.V3_7_1);
        private static readonly VersionRange Engine15Range = VersionRange.From(TinkerPopVersions.V3_8_0, TinkerPopVersions.V3_8_1);

        [Fact]
        public void Without_engine_version_and_range() => GetServerCompatibilityRange([])
            .Should()
            .Be(GremlinQuerySource.g.AsAdmin().Environment.ServerCompatibilityRange);

        [Fact]
        public void Engine_version_1_4() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.4.8.1" }
            })
            .Should()
            .Be(Engine14Range);

        [Fact]
        public void Engine_version_1_5() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" }
            })
            .Should()
            .Be(Engine15Range);

        [Fact]
        public void Range_without_engine_version() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.8.0" },
                { "Gremlinq:ServerCompatibilityRange:Highest", "3.8.2" }
            })
            .Should()
            .Be(VersionRange.From(TinkerPopVersions.V3_8_0, TinkerPopVersions.V3_8_2));

        [Fact]
        public void Engine_version_wins_over_range() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" },
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.7.1" },
                { "Gremlinq:ServerCompatibilityRange:Highest", "3.8.2" }
            })
            .Should()
            .Be(Engine15Range);

        [Fact]
        public void Engine_version_wins_over_contradicting_range() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" },
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.7.1" },
                { "Gremlinq:ServerCompatibilityRange:Highest", "3.7.7" }
            })
            .Should()
            .Be(Engine15Range);
        [Fact]
        public void Engine_version_wins_over_lowest() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" },
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.7.7" }
            })
            .Should()
            .Be(Engine15Range);

        [Fact]
        public void Lowest_above_the_default_range_throws_with_engine_version() => FluentActions
            .Invoking(() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" },
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.8.0" }
            }))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        [Fact]
        public void Unreadable_range_throws_with_engine_version() => FluentActions
            .Invoking(() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" },
                { "Gremlinq:ServerCompatibilityRange:Lowest", "3.8.x" }
            }))
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Gremlinq:ServerCompatibilityRange:Lowest*");


        [Fact]
        public void Engine_version_wins_over_highest() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.0.0" },
                { "Gremlinq:ServerCompatibilityRange:Highest", "3.8.0" }
            })
            .Should()
            .Be(Engine15Range);

        [Fact]
        public void Unreadable_engine_version_throws() => FluentActions
            .Invoking(() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.5.x" }
            }))
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Gremlinq:Neptune:EngineVersion*");

        [Fact]
        public void Unknown_engine_version_throws() => FluentActions
            .Invoking(() => GetServerCompatibilityRange(new Dictionary<string, string?>
            {
                { "Gremlinq:Neptune:EngineVersion", "1.2.0.0" }
            }))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        private static VersionRange GetServerCompatibilityRange(Dictionary<string, string?> configuration) => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(configuration)
                .Build())
            .AddGremlinq(setup => setup
                .UseNeptune<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .ServerCompatibilityRange;
    }
}
