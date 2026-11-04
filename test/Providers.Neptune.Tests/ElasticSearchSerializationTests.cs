using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class ElasticSearchSerializationTests : QueryExecutionTestsBase, IClassFixture<ElasticSearchSerializationTests.Fixture>
    {
        public sealed class Fixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .UseNeptune<Vertex, Edge>(builder => builder
                    .AtLocalhost()
                    .UseElasticSearch(new Uri("http://elastic.search.server")))
                .IgnoreCosmosDbSpecificProperties();
        }

        public ElasticSearchSerializationTests(Fixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {
        }

        [Fact]
        public async Task Where_property_contains_empty_string_with_TextP_support_strict() => await _g
            .V<MetaPropertyVertex>()
            .Where(c => c.CountryCallingCode!.Contains(""))
            .Verify();

        [Fact]
        public async Task Where_property_contains_empty_string_with_TextP_support_case_insensitive_strict() => await _g
            .V<MetaPropertyVertex>()
            .Where(c => c.CountryCallingCode!.Contains("", StringComparison.OrdinalIgnoreCase))
            .Verify();
    }
}
