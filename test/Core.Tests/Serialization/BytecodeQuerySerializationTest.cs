using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class BytecodeQuerySerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public BytecodeQuerySerializationTest(GremlinqFixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }

        [Fact]
        public virtual Task Drop() => _g
            .V<Person>()
            .Drop()
            .Verify();

        [Fact]
        public virtual Task Drop_in_local() => _g
            .Inject(1)
            .Local(__ => __
                .V()
                .Drop())
            .Verify();
    }
}
