using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinqConfiguratorExtensionsTest
    {
        [Fact]
        public void WithCompatibility_sets_the_compatibility()
        {
            var compatibility = VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2);

            GetCompatibility(GremlinQuerySource.g, new TestConfigurator().WithCompatibility(compatibility))
                .Should()
                .Be(compatibility);
        }

        [Fact]
        public void WithCompatibility_wins_over_a_compatibility_set_before_it()
        {
            var compatibility = VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2);
            var providerDefault = VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2);

            var source = GremlinQuerySource.g
                .ConfigureEnvironment(environment => environment
                    .ConfigureServerCompatibilityRange(_ => providerDefault));

            GetCompatibility(source, new TestConfigurator().WithCompatibility(compatibility))
                .Should()
                .Be(compatibility);
        }

        [Fact]
        public void WithCompatibility_sets_the_compatibility_through_a_struct_configurator()
        {
            var compatibility = VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2);

            GetCompatibility(GremlinQuerySource.g, new TestStructConfigurator().WithCompatibility(compatibility))
                .Should()
                .Be(compatibility);
        }

        private static VersionRange GetCompatibility(IGremlinQuerySource source, IGremlinQuerySourceTransformation transformation) => transformation
            .Transform(source)
            .AsAdmin()
            .Environment
            .ServerCompatibilityRange;

        private sealed class TestConfigurator : IGremlinqConfigurator<TestConfigurator>
        {
            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _transformation;

            public TestConfigurator() : this(static source => source)
            {
            }

            private TestConfigurator(Func<IGremlinQuerySource, IGremlinQuerySource> transformation)
            {
                _transformation = transformation;
            }

            public TestConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new(source => transformation(_transformation(source)));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _transformation(source);
        }

        private readonly struct TestStructConfigurator : IGremlinqConfigurator<TestStructConfigurator>
        {
            private readonly Func<IGremlinQuerySource, IGremlinQuerySource>? _transformation;

            private TestStructConfigurator(Func<IGremlinQuerySource, IGremlinQuerySource> transformation)
            {
                _transformation = transformation;
            }

            public TestStructConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation)
            {
                var self = this;

                return new(source => transformation(self.Transform(source)));
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _transformation is { } transformation
                ? transformation(source)
                : source;
        }
    }
}
