using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class DebugTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public DebugTests(CosmosDbFixture fixture) : base(
            fixture,
            new DebugGremlinQueryVerifier())
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
