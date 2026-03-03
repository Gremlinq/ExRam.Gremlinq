using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core.Tests
{
    public class RequestMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public RequestMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }

        [Fact]
        public virtual Task Tree_for_code_coverage_1() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_2() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_3() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_4() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_5() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_6() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_7() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_8() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_9() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_10() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_11() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_12() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_13() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_14() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_15() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_16() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_17() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_18() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_19() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_20() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_21() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_22() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_23() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_24() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_25() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_26() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age)
                .Of<Person>().By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Tree_for_code_coverage_27() => _g
            .V<Person>()
            .Tree(_ => _
                .Of<Person>().By(x => x.Age))
            .Verify();
    }
}
