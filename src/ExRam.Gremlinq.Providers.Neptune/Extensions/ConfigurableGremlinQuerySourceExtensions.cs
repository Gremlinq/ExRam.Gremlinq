using System;

using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class NeptuneConfigurator : INeptuneConfigurator
        {
            private readonly IWebSocketConfigurator _webSocketConfigurator;

            public NeptuneConfigurator(IWebSocketConfigurator webSocketConfigurator)
            {
                _webSocketConfigurator = webSocketConfigurator;
            }

            public INeptuneConfigurator At(Uri uri) => new NeptuneConfigurator(_webSocketConfigurator.At(uri));

            public INeptuneConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation) => new NeptuneConfigurator(transformation(_webSocketConfigurator));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _webSocketConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseNeptune(this IConfigurableGremlinQuerySource source, Func<INeptuneConfigurator, IGremlinQuerySourceTransformation> transformation)
        {
            return source
                .UseWebSocket(configurator => transformation(new NeptuneConfigurator(configurator)))
                .ConfigureEnvironment(environment => environment
                    .ConfigureSerializer(serializer => serializer
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<PropertyStep.ByKeyStep>((step, env, overridden, recurse) => overridden(Cardinality.List.Equals(step.Cardinality) ? new PropertyStep.ByKeyStep(step.Key, step.Value, step.MetaProperties, Cardinality.Set) : step, env, recurse))))
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
