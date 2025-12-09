using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class StringIdIntegrationTests : QueryExecutionTest, IClassFixture<StringIdGremlinServerContainerFixture>
    {
        public StringIdIntegrationTests(StringIdGremlinServerContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }

        [Fact(Skip = "String id nondeterminism.")]
        public override Task E_baseType_Properties() => base.E_baseType_Properties();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task E_of_all_types1() => base.E_of_all_types1();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task E_of_all_types2() => base.E_of_all_types2();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task E_of_concrete_type() => base.E_of_concrete_type();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task E_Properties() => base.E_Properties();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task Group() => base.Group();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task Group_with_key_identity() => base.Group_with_key_identity();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task Properties2() => base.Properties2();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task UpdateE_With_Ignored() => base.UpdateE_With_Ignored();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task UpdateE_With_Mixed() => base.UpdateE_With_Mixed();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task UpdateE_With_Readonly() => base.UpdateE_With_Readonly();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task V_Both_typed() => base.V_Both_typed();

        [Fact(Skip = "String id nondeterminism.")]
        public override Task V_BothV() => base.V_BothV();
    }
}
