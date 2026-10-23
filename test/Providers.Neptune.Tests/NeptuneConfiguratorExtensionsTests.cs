using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneConfiguratorExtensionsTests
    {
        private static readonly VersionRange TinkerPop37Compatibility = VersionRange.From(TinkerPopVersions.V3_7_1, TinkerPopVersions.V3_7_1);
        private static readonly VersionRange TinkerPop38Compatibility = VersionRange.From(TinkerPopVersions.V3_8_0, TinkerPopVersions.V3_8_1);

        [Fact]
        public void Without_WithEngineVersion_Neptune_keeps_the_default_compatibility() => GetCompatibility(static configurator => configurator)
            .Should()
            .Be(GremlinQuerySource.g.AsAdmin().Environment.ServerCompatibilityRange);

        [Fact]
        public void WithEngineVersion_1_3_2_0_understands_TinkerPop_3_7_1() => GetCompatibility(new Version(1, 3, 2, 0))
            .Should()
            .Be(TinkerPop37Compatibility);

        [Fact]
        public void WithEngineVersion_1_4_0_0_understands_TinkerPop_3_7_1() => GetCompatibility(new Version(1, 4, 0, 0))
            .Should()
            .Be(TinkerPop37Compatibility);

        [Fact]
        public void WithEngineVersion_1_4_8_1_understands_TinkerPop_3_7_1() => GetCompatibility(new Version(1, 4, 8, 1))
            .Should()
            .Be(TinkerPop37Compatibility);

        [Fact]
        public void WithEngineVersion_of_an_unknown_patch_1_4_9_0_understands_TinkerPop_3_7_1() => GetCompatibility(new Version(1, 4, 9, 0))
            .Should()
            .Be(TinkerPop37Compatibility);

        [Fact]
        public void WithEngineVersion_1_5_0_0_understands_TinkerPop_3_8_0_to_3_8_1() => GetCompatibility(new Version(1, 5, 0, 0))
            .Should()
            .Be(TinkerPop38Compatibility);

        [Fact]
        public void WithEngineVersion_of_an_unknown_patch_1_5_1_0_understands_TinkerPop_3_8_0_to_3_8_1() => GetCompatibility(new Version(1, 5, 1, 0))
            .Should()
            .Be(TinkerPop38Compatibility);

        [Fact]
        public void WithEngineVersion_1_3_1_0_throws() => WithEngineVersion(new Version(1, 3, 1, 0))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        [Fact]
        public void WithEngineVersion_1_2_0_0_throws() => WithEngineVersion(new Version(1, 2, 0, 0))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        [Fact]
        public void WithEngineVersion_1_6_0_0_throws() => WithEngineVersion(new Version(1, 6, 0, 0))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        [Fact]
        public void WithEngineVersion_2_0_0_0_throws() => WithEngineVersion(new Version(2, 0, 0, 0))
            .Should()
            .Throw<ArgumentOutOfRangeException>();

        [Fact]
        public void WithEngineVersion_null_throws() => WithEngineVersion(null!)
            .Should()
            .Throw<ArgumentNullException>();

        [Fact]
        public void WithCompatibility_after_WithEngineVersion_wins()
        {
            var compatibility = VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2);

            GetCompatibility(configurator => configurator
                    .WithEngineVersion(new Version(1, 5, 0, 0))
                    .WithCompatibility(compatibility))
                .Should()
                .Be(compatibility);
        }

        [Fact]
        public void WithEngineVersion_after_WithCompatibility_wins() => GetCompatibility(configurator => configurator
                .WithCompatibility(VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2))
                .WithEngineVersion(new Version(1, 5, 0, 0)))
            .Should()
            .Be(TinkerPop38Compatibility);

        private static VersionRange GetCompatibility(Version engineVersion) => GetCompatibility(configurator => configurator
            .WithEngineVersion(engineVersion));

        private static VersionRange GetCompatibility(Func<INeptuneConfigurator, IGremlinQuerySourceTransformation> configuration) => GremlinQuerySource.g
            .UseNeptune<Vertex, Edge>(configuration)
            .AsAdmin()
            .Environment
            .ServerCompatibilityRange;

        private static Action WithEngineVersion(Version engineVersion) => () => GremlinQuerySource.g
            .UseNeptune<Vertex, Edge>(configurator => configurator
                .WithEngineVersion(engineVersion));
    }
}
