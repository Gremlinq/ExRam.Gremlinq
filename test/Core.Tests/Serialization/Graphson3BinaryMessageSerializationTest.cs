using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Graphson3BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>, ISourceFileNameProvider<Graphson3BinaryMessageSerializationTest>
    {
        public Graphson3BinaryMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        [Fact]
        public Task MaxDepth() => _g
            .Inject(0)
            .Map(GetLambda(29))
            .Verify();

        [Fact]
        public Task Inject_byte() => _g
            .Inject((byte)200)
            .Verify();

        [Fact]
        public Task Inject_sbyte() => _g
            .Inject((sbyte)-56)
            .Verify();

        // Below TinkerPop 3.8, a DateTimeOffset goes out as g:Date, in milliseconds since the Unix epoch,
        // and a DateTime as a DateTimeOffset in UTC.
        [Fact]
        public Task Inject_DateTimeOffset() => _g
            .Inject(new DateTimeOffset(2023, 8, 2, 2, 0, 0, TimeSpan.FromHours(2)))
            .Verify();

        [Fact]
        public Task Inject_DateTimeOffset_with_fraction_of_a_second() => _g
            .Inject(new DateTimeOffset(2007, 12, 3, 10, 15, 30, 123, TimeSpan.FromHours(1)))
            .Verify();

        [Fact]
        public Task Inject_DateTime() => _g
            .Inject(new DateTime(2023, 8, 2, 0, 0, 0, DateTimeKind.Utc))
            .Verify();

        private Func<IGremlinQuery<int>, IGremlinQuery<int>> GetLambda(int i) => i == 0
            ? __ => __.Constant(1)
            : __ => __.Map(GetLambda(i - 1));
    }
}
