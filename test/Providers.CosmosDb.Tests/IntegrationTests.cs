using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public class IntegrationTests : QueryExecutionTest, IClassFixture<CosmosDbEmulatorFixture>
    {
        public class CosmosDbEmulatorExecutingVerifier : ExecutingVerifier
        {
            public CosmosDbEmulatorExecutingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .ScrubMember("x-ms-server-time-ms")
                .ScrubMember("x-ms-total-server-time-ms")
                .ScrubMember("x-ms-request-charge")
                .ScrubMember("x-ms-total-request-charge");
        }

        public IntegrationTests(CosmosDbEmulatorFixture fixture) : base(
            fixture,
            new CosmosDbEmulatorExecutingVerifier())
        {
        }

        [Fact(Skip = "id as key cannot be scrubbed.")]
        public override Task Group_with_key_identity() => base.Group_with_key_identity();

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
