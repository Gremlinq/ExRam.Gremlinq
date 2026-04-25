using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public abstract class CosmosDbTestBase : QueryExecutionTest
    {
        protected CosmosDbTestBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_property_contains_constant_with_TextP_support_case_insensitive() => base.Where_property_contains_constant_with_TextP_support_case_insensitive();

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_property_ends_with_constant_with_TextP_support_case_insensitive() => base.Where_property_ends_with_constant_with_TextP_support_case_insensitive();

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_property_is_prefix_of_constant_case_insensitive() => base.Where_property_is_prefix_of_constant_case_insensitive();

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_property_is_prefix_of_expression_case_insensitive() => base.Where_property_is_prefix_of_expression_case_insensitive();

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_property_is_prefix_of_variable_case_insensitive() => base.Where_property_is_prefix_of_variable_case_insensitive();

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_string_property_equals_case_insensitive() => base.Where_string_property_equals_case_insensitive();

        [Fact(Skip = "No case insensitivity on CosmosDb")]
        public override Task Where_string_property_startsWith_case_insensitive() => base.Where_string_property_startsWith_case_insensitive();
    }
}
