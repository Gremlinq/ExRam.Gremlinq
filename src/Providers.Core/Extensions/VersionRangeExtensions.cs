using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class VersionRangeExtensions
    {
        // Core's VersionRange.IsAtLeast is internal to Core: whether the lowest version of the range,
        // and with it every version of it, is at least the major and minor version given.
        public static bool IsAtLeast(this VersionRange range, Version version) => (range.Lowest.Major, range.Lowest.Minor).CompareTo((version.Major, version.Minor)) >= 0;
    }
}
