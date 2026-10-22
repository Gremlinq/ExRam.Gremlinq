using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// A range of versions, from <see cref="Lowest"/> to <see cref="Highest"/>, both included.
    /// Create one with <see cref="From(Version, Version)"/>.
    /// </summary>
    public readonly struct VersionRange : IEquatable<VersionRange>
    {
        private readonly Version? _lowest;
        private readonly Version? _highest;

        private VersionRange(Version lowest, Version highest)
        {
            _lowest = lowest;
            _highest = highest;
        }

        /// <summary>
        /// Creates the range of versions from <paramref name="lowest"/> to <paramref name="highest"/>, both included.
        /// </summary>
        /// <param name="lowest">The lowest version of the range.</param>
        /// <param name="highest">The highest version of the range.</param>
        /// <exception cref="ArgumentNullException"><paramref name="lowest"/> or <paramref name="highest"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="lowest"/> is greater than <paramref name="highest"/>.</exception>
        public static VersionRange From(Version lowest, Version highest)
        {
            ArgumentNullException.ThrowIfNull(lowest);
            ArgumentNullException.ThrowIfNull(highest);

            if (lowest.CompareTo(highest) > 0)
                throw new ArgumentOutOfRangeException(nameof(lowest), lowest, $"The lowest version must not be greater than the highest one ({highest}).");

            return new VersionRange(lowest, highest);
        }

        /// <summary>
        /// The lowest version of the range.
        /// </summary>
        public Version Lowest => _lowest ?? throw UninitializedStruct();

        /// <summary>
        /// The highest version of the range.
        /// </summary>
        public Version Highest => _highest ?? throw UninitializedStruct();

        /// <summary>
        /// Whether <see cref="Lowest"/>, and with it every version of the range, is at least the major and minor version of
        /// <paramref name="version"/>.
        /// </summary>
        internal bool IsAtLeast(Version version) => CompareMajorAndMinor(Lowest, version) >= 0;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is VersionRange compatibility && Equals(compatibility);

        /// <inheritdoc />
        public bool Equals(VersionRange other) => _lowest == other._lowest && _highest == other._highest;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(_lowest, _highest);

        /// <summary>Tests two <see cref="VersionRange"/> instances for equality.</summary>
        public static bool operator ==(VersionRange left, VersionRange right) => left.Equals(right);

        /// <summary>Tests two <see cref="VersionRange"/> instances for inequality.</summary>
        public static bool operator !=(VersionRange left, VersionRange right) => !(left == right);

        private static int CompareMajorAndMinor(Version left, Version right) => (left.Major, left.Minor).CompareTo((right.Major, right.Minor));
    }
}
