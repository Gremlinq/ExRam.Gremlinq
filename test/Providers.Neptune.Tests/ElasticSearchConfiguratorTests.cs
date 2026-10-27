using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class ElasticSearchConfiguratorTests
    {
        [Fact]
        public void LowercaseKeyword_StartsWith_escapes_whitespace() => LowercaseKeyword(static configurator => configurator)
            .V<MetaPropertyVertex>()
            .Where(c => c.CountryCallingCode!.StartsWith("a b", StringComparison.OrdinalIgnoreCase))
            .Debug()
            .Should()
            .Contain(@".has('CountryCallingCode','Neptune#fts a\ b*')");

        [Fact]
        public void LowercaseKeyword_EndsWith_escapes_whitespace() => LowercaseKeyword(static configurator => configurator)
            .V<MetaPropertyVertex>()
            .Where(c => c.CountryCallingCode!.EndsWith("a b", StringComparison.OrdinalIgnoreCase))
            .Debug()
            .Should()
            .Contain(@".has('CountryCallingCode','Neptune#fts *a\ b')");

        [Fact]
        public void LowercaseKeyword_Contains_escapes_whitespace() => LowercaseKeyword(static configurator => configurator)
            .V<MetaPropertyVertex>()
            .Where(c => c.CountryCallingCode!.Contains("a b", StringComparison.OrdinalIgnoreCase))
            .Debug()
            .Should()
            .Contain(@".has('CountryCallingCode','Neptune#fts *a\ b*')");

        [Fact]
        public void ConfigureQuerySource_keeps_ElasticSearch() => LowercaseKeyword(static configurator => configurator
                .ConfigureQuerySource(source => source.WithSideEffect("x", 1)))
            .V()
            .Debug()
            .Should()
            .StartWith("g.withSideEffect('x',1).withSideEffect('Neptune#fts.endpoint','http://elastic.search.server').withSideEffect('Neptune#fts.queryType','query_string').V()");

        [Fact]
        public void ConfigureClientFactory_keeps_ElasticSearch() => LowercaseKeyword(static configurator => configurator
                .ConfigureClientFactory(factory => factory))
            .V()
            .Debug()
            .Should()
            .StartWith("g.withSideEffect('Neptune#fts.endpoint','http://elastic.search.server').withSideEffect('Neptune#fts.queryType','query_string').V()");

        private static IGremlinQuerySource LowercaseKeyword(Func<INeptuneConfigurator, INeptuneConfigurator> transformation) => GremlinQuerySource.g
            .UseNeptune<Vertex, Edge>(configurator => transformation(configurator
                .AtLocalhost()
                .UseElasticSearch(new Uri("http://elastic.search.server"), NeptuneElasticSearchIndexConfiguration.LowercaseKeyword)));
    }
}
