using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static IGremlinQueryEnvironment UseNeptune(this IGremlinQueryEnvironment environment, Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> transformation)
        {
            return environment
                .UseWebSocket(transformation)
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
