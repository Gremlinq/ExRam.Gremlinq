using System;

using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class JanusGraphConfigurator : IJanusGraphConfigurator
        {
            private readonly WebSocketProviderConfigurator _baseConfigurator;

            public JanusGraphConfigurator() : this(new WebSocketProviderConfigurator())
            {
            }

            public JanusGraphConfigurator(WebSocketProviderConfigurator baseConfigurator)
            {
                _baseConfigurator = baseConfigurator;
            }

            public IJanusGraphConfigurator ConfigureAlias(Func<string, string> transformation) => new JanusGraphConfigurator(_baseConfigurator.ConfigureAlias(transformation));

            public IJanusGraphConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new JanusGraphConfigurator(_baseConfigurator.ConfigureClientFactory(transformation));

            public IJanusGraphConfigurator ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new JanusGraphConfigurator(_baseConfigurator.ConfigureServer(transformation));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _baseConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseJanusGraph(this IConfigurableGremlinQuerySource source, Func<IJanusGraphConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(new JanusGraphConfigurator())
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(_ => GraphFeatures.Computer | GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.Persistence)
                            .ConfigureVariableFeatures(_ => VariableFeatures.MapValues)
                            .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                            .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                            .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                            .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                        .UseGraphSon3()
                        .UseNewtonsoftJson()))
                .ConfigureEnvironment(environment => environment
                    .StoreByteArraysAsBase64String());
        }
    }
}
