using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public static class NeptuneConfiguratorExtensions
    {
        private sealed class ElasticSearchAwareNeptuneConfigurator : INeptuneConfigurator
        {
            private sealed class ElasticSearchAwarePFactory : IPFactory
            {
                private readonly NeptuneElasticSearchIndexConfiguration _indexConfiguration;

                public ElasticSearchAwarePFactory(NeptuneElasticSearchIndexConfiguration indexConfiguration)
                {
                    _indexConfiguration = indexConfiguration;
                }

                public P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment)
                {
                    if (value is string {Length: > 0} str && semantics is StringExpressionSemantics {Comparison: StringComparison.OrdinalIgnoreCase} stringExpressionSemantics)
                    {
                        switch (_indexConfiguration)
                        {
                            case NeptuneElasticSearchIndexConfiguration.Standard:
                            {
                                if (!str.Any(char.IsWhiteSpace)) //Can't do better. Insight welcome.
                                {
                                    switch (stringExpressionSemantics)
                                    {
                                        // This will only work for property values that don't contain e.g. whitespace
                                        // and would be tokenized as a complete string. As it is, a vertex property
                                        // with value "John Doe" would match a query "StartsWith('Doe') which is
                                        // not really what's expected. So we can't do better than a case-insensitive
                                        // "Contains(..)"
                                        case HasInfixExpressionSemantics:
                                            return new P("eq", $"Neptune#fts *{value}*");
                                    }
                                }

                                break;
                            }
                            case NeptuneElasticSearchIndexConfiguration.LowercaseKeyword:
                            {
                                str = str
                                    .Replace(" ", @"\ ");

                                switch (stringExpressionSemantics)
                                {
                                    case StartsWithExpressionSemantics:
                                        return new P("eq", $"Neptune#fts {str}*");
                                    case EndsWithExpressionSemantics:
                                        return new P("eq", $"Neptune#fts *{str}");
                                    case HasInfixExpressionSemantics:
                                        return new P("eq", $"Neptune#fts *{str}*");
                                }

                                break;
                            }
                        }
                    }

                    return default;
                }
            }

            private readonly Uri _elasticSearchEndPoint;
            private readonly INeptuneConfigurator _baseConfigurator;
            private readonly NeptuneElasticSearchIndexConfiguration _indexConfiguration;

            public ElasticSearchAwareNeptuneConfigurator(INeptuneConfigurator baseConfigurator, Uri elasticSearchEndPoint, NeptuneElasticSearchIndexConfiguration indexConfiguration)
            {
                _baseConfigurator = baseConfigurator;
                _indexConfiguration = indexConfiguration;
                _elasticSearchEndPoint = elasticSearchEndPoint;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _baseConfigurator
                .Transform(source)
                .WithSideEffect("Neptune#fts.endpoint", _elasticSearchEndPoint.OriginalString)
                .WithSideEffect("Neptune#fts.queryType", "query_string")
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .ConfigureValue(
                            PFactory.PFactoryOption,
                            factory => factory.Override(new ElasticSearchAwarePFactory(_indexConfiguration)))));

            public INeptuneConfigurator ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> transformation) => new ElasticSearchAwareNeptuneConfigurator(
                _baseConfigurator.ConfigureClientFactory(transformation),
                _elasticSearchEndPoint,
                _indexConfiguration);

            public INeptuneConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new ElasticSearchAwareNeptuneConfigurator(
                _baseConfigurator.ConfigureQuerySource(transformation),
                _elasticSearchEndPoint,
                _indexConfiguration);
        }

        public static INeptuneConfigurator UseElasticSearch(this INeptuneConfigurator configurator, Uri elasticSearchEndPoint, NeptuneElasticSearchIndexConfiguration indexConfiguration = NeptuneElasticSearchIndexConfiguration.Standard) => new ElasticSearchAwareNeptuneConfigurator(
            configurator,
            elasticSearchEndPoint,
            indexConfiguration);
    }
}
