using System;

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
                private readonly IPFactory _baseFactory;

                public ElasticSearchAwarePFactory(IPFactory baseFactory)
                {
                    _baseFactory = baseFactory;
                }

                public P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment)
                {
                    if (semantics is StringExpressionSemantics { Comparison: StringComparison.OrdinalIgnoreCase } stringExpressionSemantics)
                    {
                        return stringExpressionSemantics switch
                        {
                            StartsWithExpressionSemantics startsWith => P.Eq($"Neptune#fts {value}*"),
                            EndsWithExpressionSemantics endsWith => P.Eq($"Neptune#fts *{value}"),
                            HasInfixExpressionSemantics hasInfix => P.Eq($"Neptune#fts *{value}*"),
                            _ => null
                        };
                    }
                    
                    return _baseFactory.TryGetP(semantics, value, environment);
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

            public IGremlinQuerySourceTransformation UseElasticSearch(Uri endPoint)
            {
                return new NeptuneConfigurator(_webSocketBuilder, endPoint);
            }

            public IGremlinQuerySourceTransformation ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
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
                        .ConfigureEnvironment(env => env
                            .ConfigureOptions(options => options
                                .ConfigureValue(
                                    PFactory.PFactoryOption,
                                    factory => new ElasticSearchAwarePFactory(factory))));
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
