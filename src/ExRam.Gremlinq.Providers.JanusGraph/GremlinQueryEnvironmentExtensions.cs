﻿using System;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class JanusGraphConfigurator :
            IJanusGraphConfigurator,
            IJanusGraphConfiguratorWithUri
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

            public IGremlinQueryEnvironmentTransformation ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                return new JanusGraphConfigurator(
                    transformation(_webSocketBuilder));
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return _webSocketBuilder.Transform(environment);
            }
        }

        public static IGremlinQueryEnvironment UseJanusGraph(this IGremlinQueryEnvironment environment, Func<IJanusGraphConfigurator, IGremlinQueryEnvironmentTransformation> transformation)
        {
            return environment
                .UseGremlinServer(builder => transformation(new JanusGraphConfigurator(builder)))
                .ConfigureFeatureSet(featureSet => featureSet
                    .ConfigureGraphFeatures(_ => GraphFeatures.Computer | GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.Persistence)
                    .ConfigureVariableFeatures(_ => VariableFeatures.MapValues)
                    .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                    .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                    .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                    .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                .ConfigureModel(model => model
                    .ConfigureNativeTypes(types => types
                        .Remove(typeof(byte[]))))
                .ConfigureSerializer(_ => _
                    .ConfigureFragmentSerializer(_ => _
                        .Override<byte[]>((bytes, env, overridden, recurse) => recurse.Serialize(Convert.ToBase64String(bytes), env))));
        }
    }
}
