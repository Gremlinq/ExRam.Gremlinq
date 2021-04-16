using System;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class JanusGraphConfigurator : IJanusGraphConfigurator, IJanusGraphConfiguratorWithUri
        {
            private readonly IWebSocketConfigurator _webSocketBuilder;

            public JanusGraphConfigurator(IWebSocketConfigurator webSocketBuilder)
            {
                _webSocketBuilder = webSocketBuilder;
            }

            public IJanusGraphConfiguratorWithUri At(Uri uri)
            {
                return new JanusGraphConfigurator(_webSocketBuilder.At(uri));
            }

            public IGremlinQuerySourceTransformation ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                return new JanusGraphConfigurator(
                    transformation(_webSocketBuilder));
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _webSocketBuilder
                    .Transform(source);
            }
        }

        public static IGremlinQuerySource UseJanusGraph(this IConfigurableGremlinQuerySource environment, Func<IJanusGraphConfigurator, IGremlinQuerySourceTransformation> transformation)
        {
            return environment
                .UseGremlinServer(builder => transformation(new JanusGraphConfigurator(builder)))
                .ConfigureEnvironment(environment => environment
                    .ConfigureFeatureSet(featureSet => featureSet
                        .ConfigureGraphFeatures(_ => GraphFeatures.Computer | GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.Persistence)
                        .ConfigureVariableFeatures(_ => VariableFeatures.MapValues)
                        .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                        .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                        .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                        .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                    .StoreByteArraysAsBase64String());
        }
    }
}
