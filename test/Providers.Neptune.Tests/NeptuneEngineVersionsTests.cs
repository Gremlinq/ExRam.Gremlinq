using System.Reflection;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneEngineVersionsTests
    {
        [Fact]
        public void Every_version_is_named_after_its_value()
        {
            foreach (var field in typeof(NeptuneEngineVersions).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                ((Version)field.GetValue(null)!)
                    .ToString()
                    .Should()
                    .Be(field.Name[1..].Replace('_', '.'));
            }
        }

        [Fact]
        public void Versions_run_from_1_3_2_0_to_1_5_0_0()
        {
            var versions = GetVersions();

            versions
                .Should()
                .HaveCount(20)
                .And
                .OnlyHaveUniqueItems();

            versions[0]
                .Should()
                .Be(new Version(1, 3, 2, 0));

            versions[^1]
                .Should()
                .Be(new Version(1, 5, 0, 0));
        }

        [Fact]
        public void WithEngineVersion_knows_every_version()
        {
            foreach (var version in GetVersions())
            {
                GremlinQuerySource.g
                    .UseNeptune<Vertex, Edge>(configurator => configurator
                        .WithEngineVersion(version))
                    .AsAdmin()
                    .Environment
                    .ServerCompatibilityRange
                    .Should()
                    .Be(version.Minor < 5
                        ? VersionRange.From(TinkerPopVersions.V3_7_1, TinkerPopVersions.V3_7_1)
                        : VersionRange.From(TinkerPopVersions.V3_8_0, TinkerPopVersions.V3_8_1));
            }
        }

        private static Version[] GetVersions() => typeof(NeptuneEngineVersions)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(static field => (Version)field.GetValue(null)!)
            .Order()
            .ToArray();
    }
}
