using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class JanusGraphConfigurator : IJanusGraphConfigurator
        {
            public static readonly JanusGraphConfigurator Default = new (ProviderConfigurator<IGremlinqClientFactory>.Default);

            private readonly ProviderConfigurator<IGremlinqClientFactory> _webSocketProviderConfigurator;

            private JanusGraphConfigurator(ProviderConfigurator<IGremlinqClientFactory> webSocketProviderConfigurator)
            {
                _webSocketProviderConfigurator = webSocketProviderConfigurator;
            }

            public IJanusGraphConfigurator ConfigureClientFactory(Func<IGremlinqClientFactory, IGremlinqClientFactory> transformation) => new JanusGraphConfigurator(_webSocketProviderConfigurator.ConfigureClientFactory(transformation));

            public IJanusGraphConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new JanusGraphConfigurator(_webSocketProviderConfigurator.ConfigureQuerySource(transformation));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _webSocketProviderConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<IJanusGraphConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(JanusGraphConfigurator.Default)
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .UseModel(GraphModel
                            .FromBaseTypes<TVertexBase, TEdgeBase>())
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(_ => GraphFeatures.Computer | GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.Persistence)
                            .ConfigureVariableFeatures(_ => VariableFeatures.MapValues)
                            .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                            .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                            .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                            .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                        .ConfigureNativeTypes(nativeTypes => nativeTypes
                            .Remove(typeof(byte[])))
                        .UseGraphSon3()));
        }
    }
}
