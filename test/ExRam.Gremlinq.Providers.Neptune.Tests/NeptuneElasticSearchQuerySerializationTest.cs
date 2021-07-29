using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneElasticSearchQuerySerializationTest : QuerySerializationTest, IClassFixture<NeptuneElasticSearchQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : QuerySerializationTest.Fixture
        {
            public Fixture() : base(g
                .UseNeptune(builder => builder
                    .AtLocalhost()
                    .UseElasticSearch(new Uri("http://elastic.search.server"))))
            {
            }
        }

        public NeptuneElasticSearchQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
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
