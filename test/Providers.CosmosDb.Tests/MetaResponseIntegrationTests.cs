using System.Text.RegularExpressions;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public partial class MetaResponseIntegrationTests : QueryExecutionTest, IClassFixture<CosmosDbEmulatorFixture>
    {
        private static readonly Regex DoubleRegex = DoubleRegexRegexImpl();

        private sealed class MetaResponseExecutingVerifier : ExecutingVerifier
        {
            public MetaResponseExecutingVerifier() : base()
            {

            }

            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base
                .Verify(query.Cast<MetaResponse<TElement>>());

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task);
                //.ScrubRegex(DoubleRegex, "(double)");
        }

        public MetaResponseIntegrationTests(CosmosDbEmulatorFixture fixture) : base(
            fixture,
            new MetaResponseExecutingVerifier())
        {
        }

        [GeneratedRegex(@"\d+\.\d+")]
        private static partial Regex DoubleRegexRegexImpl();


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
