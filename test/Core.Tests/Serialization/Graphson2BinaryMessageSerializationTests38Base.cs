using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// What a GraphSON 2 writer sends where TinkerPop 3.8 writes a type in another form than 3.7, for a server from TinkerPop 3.8
    /// on, answering like <see cref="Graphson2BinaryMessageSerializationTests38"/>, the leaf that owns the snapshots. A range that
    /// spans both is sent the 3.7 form, which <see cref="Graphson2BinaryMessageSerializationTests37To38"/> verifies. The
    /// fixture sets the range to 3.8.2.
    /// </summary>
    public abstract class Graphson2BinaryMessageSerializationTests38Base : GremlinqTestBase
    {
        protected readonly IGremlinQuerySource _g;

        protected Graphson2BinaryMessageSerializationTests38Base(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(verifier.WithSnapshotsOf<Graphson2BinaryMessageSerializationTests38>())
        {
            _g = fixture.GetQuerySource();
        }

        // TinkerPop's Byte is signed (TINKERPOP-3161): an sbyte goes out as gx:Byte, a byte as a bare number.
        [Fact]
        public virtual SettingsTask Inject_byte() => _g
            .Inject((byte)200)
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_sbyte() => _g
            .Inject((sbyte)-56)
            .Verify();

        // A DateTimeOffset goes out as gx:OffsetDateTime with its offset, seconds always and a fraction
        // of a second only when there is one, as in https://tinkerpop.apache.org/docs/3.8.0/dev/io/.
        // A DateTime goes out as a DateTimeOffset in UTC.
        [Fact]
        public virtual SettingsTask Inject_DateTimeOffset() => _g
            .Inject(new DateTimeOffset(2023, 8, 2, 2, 0, 0, TimeSpan.FromHours(2)))
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_DateTimeOffset_in_UTC() => _g
            .Inject(new DateTimeOffset(2023, 8, 2, 0, 0, 0, TimeSpan.Zero))
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_DateTimeOffset_with_fraction_of_a_second() => _g
            .Inject(new DateTimeOffset(2007, 12, 3, 10, 15, 30, 123, TimeSpan.FromHours(1)))
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_DateTime() => _g
            .Inject(new DateTime(2023, 8, 2, 0, 0, 0, DateTimeKind.Utc))
            .Verify();
    }
}
