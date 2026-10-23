using System.Reflection;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TinkerPopVersionsTest
    {
        [Fact]
        public void Every_version_is_named_after_its_value()
        {
            foreach (var field in typeof(TinkerPopVersions).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                ((Version)field.GetValue(null)!)
                    .ToString()
                    .Should()
                    .Be(field.Name[1..].Replace('_', '.'));
            }
        }

        [Fact]
        public void Versions_run_from_3_1_3_to_3_8_2()
        {
            var versions = typeof(TinkerPopVersions)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(static field => (Version)field.GetValue(null)!)
                .Order()
                .ToArray();

            versions
                .Should()
                .HaveCount(72)
                .And
                .OnlyHaveUniqueItems();

            versions[0]
                .Should()
                .Be(new Version(3, 1, 3));

            versions[^1]
                .Should()
                .Be(new Version(3, 8, 2));
        }
    }
}
