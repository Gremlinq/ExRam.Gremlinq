using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> written as a GraphSON 3 request for a server below TinkerPop 3.8, answering
    /// like <see cref="Graphson3BinaryMessageSerializationTests"/>, the leaf that owns the snapshots. Another GraphSON 3 writer
    /// derives from here and overrides the facts it writes differently.
    /// </summary>
    public abstract class Graphson3BinaryMessageSerializationTestsBase : QueryExecutionTestsBase
    {
        protected Graphson3BinaryMessageSerializationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<Graphson3BinaryMessageSerializationTests>())
        {
        }

        [Fact]
        public virtual SettingsTask MaxDepth() => _g
            .Inject(0)
            .Map(GetLambda(29))
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_byte() => _g
            .Inject((byte)200)
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_sbyte() => _g
            .Inject((sbyte)-56)
            .Verify();

        // Below TinkerPop 3.8, a DateTimeOffset goes out as g:Date, in milliseconds since the Unix epoch,
        // and a DateTime as a DateTimeOffset in UTC.
        [Fact]
        public virtual SettingsTask Inject_DateTimeOffset() => _g
            .Inject(new DateTimeOffset(2023, 8, 2, 2, 0, 0, TimeSpan.FromHours(2)))
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_DateTimeOffset_with_fraction_of_a_second() => _g
            .Inject(new DateTimeOffset(2007, 12, 3, 10, 15, 30, 123, TimeSpan.FromHours(1)))
            .Verify();

        [Fact]
        public virtual SettingsTask Inject_DateTime() => _g
            .Inject(new DateTime(2023, 8, 2, 0, 0, 0, DateTimeKind.Utc))
            .Verify();

        private Func<IGremlinQuery<int>, IGremlinQuery<int>> GetLambda(int i) => i == 0
            ? __ => __.Constant(1)
            : __ => __.Map(GetLambda(i - 1));
    }
}
