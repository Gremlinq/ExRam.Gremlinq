using System;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class NeptuneConfigurator : INeptuneConfigurator
        {
            private readonly IWebSocketConfigurator _webSocketBuilder;

            public NeptuneConfigurator(IWebSocketConfigurator webSocketBuilder)
            {
                _webSocketBuilder = webSocketBuilder;
            }

            public INeptuneConfigurator At(Uri uri)
            {
                return new NeptuneConfigurator(_webSocketBuilder.At(uri));
            }
            
            public INeptuneConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                return new NeptuneConfigurator(
                    transformation(_webSocketBuilder));
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _webSocketBuilder
                    .Transform(source);
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
