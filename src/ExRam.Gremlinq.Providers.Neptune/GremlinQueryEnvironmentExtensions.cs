using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class NeptuneConfigurationBuilder :
            INeptuneConfigurationBuilder,
            INeptuneConfigurationBuilderWithUri
        {
            private readonly IWebSocketGremlinQueryExecutorBuilder _webSocketBuilder;

            public NeptuneConfigurationBuilder(IWebSocketGremlinQueryExecutorBuilder webSocketBuilder)
            {
                _webSocketBuilder = webSocketBuilder;
            }

            public INeptuneConfigurationBuilderWithUri At(Uri uri)
            {
                return new NeptuneConfigurationBuilder(_webSocketBuilder.At(uri));
            }

            public IGremlinQueryExecutorBuilder ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation)
            {
                return new NeptuneConfigurationBuilder(
                    transformation(_webSocketBuilder));
            }

            public IGremlinQueryExecutor Build()
            {
                return _webSocketBuilder.Build();
            }

            public IGremlinQueryEnvironment Environment => _webSocketBuilder.Environment;
        }

        public static IGremlinQueryEnvironment UseNeptune(this IGremlinQueryEnvironment environment, Func<INeptuneConfigurationBuilder, IGremlinQueryExecutorBuilder> transformation)
        {
            return environment
                .UseWebSocket(builder => transformation(new NeptuneConfigurationBuilder(builder)))
                .ConfigureSerializer(serializer => serializer
                    .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                        .Override<PropertyStep>((step, env, overridden, recurse) => overridden(Cardinality.List.Equals(step.Cardinality) ? new PropertyStep(step.Key, step.Value, step.MetaProperties, Cardinality.Set) : step, env, recurse))))
                .ConfigureFeatureSet(featureSet => featureSet
                    .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                    .ConfigureVariableFeatures(_ => VariableFeatures.None)
                    .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                    .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                    .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                    .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues));
        }
    }
}
