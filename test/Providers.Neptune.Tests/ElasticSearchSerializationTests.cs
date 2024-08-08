using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class ElasticSearchSerializationTests : QueryExecutionTest, IClassFixture<ElasticSearchNeptuneFixture>
    {
        public ElasticSearchSerializationTests(ElasticSearchNeptuneFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {
        }

        [Fact]
        public async Task Where_property_contains_empty_string_with_TextP_support_strict()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.StringComparisonTranslationStrictness, StringComparisonTranslationStrictness.Strict)))
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains(""))
                .Verify();
        }

        [Fact]
        public async Task Where_property_contains_empty_string_with_TextP_support_case_insensitive_strict()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.StringComparisonTranslationStrictness, StringComparisonTranslationStrictness.Strict)))
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains("", StringComparison.OrdinalIgnoreCase))
                .Verify();
        }
    }
}
