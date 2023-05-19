using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core.Tests.Fixtures;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class ElasticSearchSerializationTests : QueryExecutionTest, IClassFixture<ElasticSearchSerializationTests.ElasticSearchFixture>
    {
        public sealed class ElasticSearchFixture : SerializationFixture<Bytecode>
        {
            public ElasticSearchFixture() : base(g
                .UseNeptune(builder => builder
                    .AtLocalhost()
                    .UseElasticSearch(new Uri("http://elastic.search.server"))))
            {
            }
        }

        public ElasticSearchSerializationTests(ElasticSearchFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
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
