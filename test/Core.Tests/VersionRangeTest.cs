using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class VersionRangeTest
    {
        private static readonly Version Version38 = new(3, 8);

        [Fact]
        public void From_throws_for_a_null_lowest_version()
        {
            var from = () => VersionRange.From(null!, new Version(3, 8, 2));

            from
                .Should()
                .Throw<ArgumentNullException>()
                .WithParameterName("lowest");
        }

        [Fact]
        public void From_throws_for_a_null_highest_version()
        {
            var from = () => VersionRange.From(new Version(3, 7, 7), null!);

            from
                .Should()
                .Throw<ArgumentNullException>()
                .WithParameterName("highest");
        }

        [Fact]
        public void From_throws_for_an_inverted_range()
        {
            var from = () => VersionRange.From(new Version(3, 8, 2), new Version(3, 7, 7));

            from
                .Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName("lowest");
        }

        [Fact]
        public void From_keeps_the_versions()
        {
            var range = VersionRange.From(new Version(3, 7), new Version(3, 8, 2, 1));

            range.Lowest
                .Should()
                .Be(new Version(3, 7));

            range.Highest
                .Should()
                .Be(new Version(3, 8, 2, 1));
        }

        [Fact]
        public void From_accepts_a_single_version()
        {
            var range = VersionRange.From(new Version(3, 8, 2), new Version(3, 8, 2));

            range.Lowest
                .Should()
                .Be(range.Highest);
        }

        [Theory]
        [InlineData("3.7.1", "3.7.7")]
        [InlineData("3.7", "3.7")]
        [InlineData("3.6.0.1", "3.7.7.9")]
        [InlineData("3.7.7", "3.8.2")]
        [InlineData("3.7", "4.0")]
        public void Range_from_below_3_8_is_not_at_least_3_8(string lowest, string highest)
        {
            VersionRange.From(Version.Parse(lowest), Version.Parse(highest))
                .IsAtLeast(Version38)
                .Should()
                .BeFalse();
        }

        [Theory]
        [InlineData("3.8", "3.8")]
        [InlineData("3.8.0", "3.8.2")]
        [InlineData("3.8.2.1", "3.8.2.1")]
        [InlineData("3.8", "4.0")]
        public void Range_from_3_8_is_at_least_3_8(string lowest, string highest)
        {
            VersionRange.From(Version.Parse(lowest), Version.Parse(highest))
                .IsAtLeast(Version38)
                .Should()
                .BeTrue();
        }

        [Theory]
        [InlineData("3.8")]
        [InlineData("3.8.0")]
        [InlineData("3.8.2.1")]
        public void IsAtLeast_compares_major_and_minor_version_only(string version)
        {
            VersionRange.From(new Version(3, 8, 2), new Version(3, 8, 2))
                .IsAtLeast(Version.Parse(version))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Uninitialized_Lowest_throws()
        {
            var range = default(VersionRange);

            range
                .Invoking(range => range.Lowest)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Uninitialized_Highest_throws()
        {
            var range = default(VersionRange);

            range
                .Invoking(range => range.Highest)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void IsAtLeast_throws_when_uninitialized()
        {
            var range = default(VersionRange);

            range
                .Invoking(range => range.IsAtLeast(Version38))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Ranges_with_the_same_bounds_are_equal()
        {
            var range = VersionRange.From(new Version(3, 7, 7), new Version(3, 8, 2));
            var other = VersionRange.From(new Version(3, 7, 7), new Version(3, 8, 2));

            (range == other)
                .Should()
                .BeTrue();

            range
                .Equals((object)other)
                .Should()
                .BeTrue();

            range
                .GetHashCode()
                .Should()
                .Be(other.GetHashCode());
        }

        [Theory]
        [InlineData("3.7.1", "3.8.2")]
        [InlineData("3.7.7", "3.8.1")]
        public void Ranges_with_other_bounds_are_not_equal(string lowest, string highest)
        {
            var range = VersionRange.From(new Version(3, 7, 7), new Version(3, 8, 2));
            var other = VersionRange.From(Version.Parse(lowest), Version.Parse(highest));

            (range != other)
                .Should()
                .BeTrue();

            range
                .Equals((object)other)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Range_is_not_equal_to_another_type()
        {
            VersionRange.From(new Version(3, 7, 1), new Version(3, 7, 7))
                .Equals(new Version(3, 7, 7))
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Uninitialized_ranges_are_equal()
        {
            (default(VersionRange) == default)
                .Should()
                .BeTrue();

            (default(VersionRange) == VersionRange.From(new Version(3, 7, 1), new Version(3, 7, 7)))
                .Should()
                .BeFalse();
        }
    }
}
