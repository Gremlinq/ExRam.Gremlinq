using System;
using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Providers.WebSocket;
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
                                if (!str.Any(c => char.IsWhiteSpace(c))) //Can't do better. Insight welcome.
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

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _baseConfigurator
                    .Transform(source)
                    .WithSideEffect("Neptune#fts.endpoint", _elasticSearchEndPoint.OriginalString)
                    .WithSideEffect("Neptune#fts.queryType", "query_string")
                    .ConfigureEnvironment(env => env
                        .ConfigureOptions(options => options
                            .ConfigureValue(
                                PFactory.PFactoryOption,
                                factory => factory
                                    .Override(new ElasticSearchAwarePFactory(_indexConfiguration)))));
            }

            public INeptuneConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                return new ElasticSearchAwareNeptuneConfigurator(
                    _baseConfigurator.ConfigureWebSocket(transformation),
                    _elasticSearchEndPoint,
                    _indexConfiguration);
            }

            public INeptuneConfigurator At(Uri uri)
            {
                return new ElasticSearchAwareNeptuneConfigurator(
                    _baseConfigurator.At(uri),
                    _elasticSearchEndPoint,
                    _indexConfiguration);
            }
        }

        public static INeptuneConfigurator UseElasticSearch(this INeptuneConfigurator configurator, Uri elasticSearchEndPoint, NeptuneElasticSearchIndexConfiguration indexConfiguration = NeptuneElasticSearchIndexConfiguration.Standard)
        {
            return new ElasticSearchAwareNeptuneConfigurator(
                configurator,
                elasticSearchEndPoint,
                indexConfiguration);
        }
    }
}
