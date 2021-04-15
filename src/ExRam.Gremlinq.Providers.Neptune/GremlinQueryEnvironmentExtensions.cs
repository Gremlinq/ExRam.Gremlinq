using System;
using System.Linq;
using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class NeptuneConfigurator : INeptuneConfigurator, INeptuneConfiguratorWithUri
        {
            private sealed class ElasticSearchAwarePFactory : IPFactory
            {
                public P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment)
                {
                    if (value is string { Length: > 0 } str && semantics is StringExpressionSemantics { Comparison: StringComparison.OrdinalIgnoreCase } stringExpressionSemantics)
                    {
                        if (!str.Any(c => char.IsWhiteSpace(c)))    //Can't do better. Insight welcome.
                        {
                            switch (stringExpressionSemantics)
                            {
                                // This will only work for property values that don't contain e.g. whitespace
                                // and would be tokenized as a complete string. As it is, a vertex property
                                // with value "John Doe" would match a query "StartsWith('Doe') which is
                                // not really what's expected. So we can't do better than a case-insensitive
                                // "Contains(..)"
                                //case StartsWithExpressionSemantics:
                                //    return new P("eq", $"Neptune#fts {value}*");
                                //case EndsWithExpressionSemantics:
                                //    return new P("eq", $"Neptune#fts *{value}");
                                case HasInfixExpressionSemantics:
                                    return new P("eq", $"Neptune#fts *{value}*");
                            }
                        }
                    }

                    return default;
                }
            }

            private readonly Uri? _elasticSearchEndPoint;
            private readonly IWebSocketConfigurator _webSocketBuilder;

            public NeptuneConfigurator(IWebSocketConfigurator webSocketBuilder, Uri? elasticSearchEndPoint = default)
            {
                _webSocketBuilder = webSocketBuilder;
                _elasticSearchEndPoint = elasticSearchEndPoint;
            }

            public INeptuneConfiguratorWithUri At(Uri uri)
            {
                return new NeptuneConfigurator(_webSocketBuilder.At(uri));
            }

            public INeptuneConfiguratorWithUri UseElasticSearch(Uri endPoint)
            {
                return new NeptuneConfigurator(_webSocketBuilder, endPoint);
            }

            public INeptuneConfiguratorWithUri ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                return new NeptuneConfigurator(
                    transformation(_webSocketBuilder));
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                var ret = _webSocketBuilder
                    .Transform(source);

                if (_elasticSearchEndPoint is { } endPoint)
                {
                    ret = ret
                        .WithSideEffect("Neptune#fts.endpoint", endPoint.ToString())
                        .WithSideEffect("Neptune#fts.queryType", "query_string")
                        .ConfigureEnvironment(env => env
                            .ConfigureOptions(options => options
                                .ConfigureValue(
                                    PFactory.PFactoryOption,
                                    factory => factory
                                        .Override(new ElasticSearchAwarePFactory()))));
                }

                return ret;
            }
        }

        public static IGremlinQuerySource UseNeptune(this IConfigurableGremlinQuerySource source, Func<INeptuneConfigurator, IGremlinQuerySourceTransformation> transformation)
        {
            return source
                .UseWebSocket(builder => transformation(new NeptuneConfigurator(builder)))
                .ConfigureEnvironment(environment => environment
                    .ConfigureSerializer(serializer => serializer
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<PropertyStep>((step, env, overridden, recurse) => overridden(Cardinality.List.Equals(step.Cardinality) ? new PropertyStep(step.Key, step.Value, step.MetaProperties, Cardinality.Set) : step, env, recurse))))
                    .StoreTimeSpansAsNumbers()
                    .StoreByteArraysAsBase64String()
                    .ConfigureFeatureSet(featureSet => featureSet
                        .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                        .ConfigureVariableFeatures(_ => VariableFeatures.None)
                        .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                        .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                        .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                        .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues)));
        }
    }
}
