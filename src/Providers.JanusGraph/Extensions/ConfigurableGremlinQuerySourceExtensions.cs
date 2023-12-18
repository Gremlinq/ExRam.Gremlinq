using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class JanusGraphConfigurator : IJanusGraphConfigurator
        {
            public static readonly JanusGraphConfigurator Default = new(WebSocketGremlinqClientFactory.LocalHost.Pool(), _ => _);

            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _querySourceTransformation;
            private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> _clientFactory;

            private JanusGraphConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory, Func<IGremlinQuerySource, IGremlinQuerySource> querySourceTransformation)
            {
                _clientFactory = clientFactory;
                _querySourceTransformation = querySourceTransformation;
            }

            public IJanusGraphConfigurator ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> transformation) => new JanusGraphConfigurator(
                transformation(_clientFactory),
                _querySourceTransformation);

            public IJanusGraphConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new JanusGraphConfigurator(
                _clientFactory,
                _ => transformation(_querySourceTransformation(_)));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _querySourceTransformation
                .Invoke(source
                    .ConfigureEnvironment(environment => environment
                        .UseExecutor(_clientFactory
                            .Log()
                            .ToExecutor())));
        }

        public static IGremlinQuerySource UseJanusGraph<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<IJanusGraphConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(JanusGraphConfigurator.Default)
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .AddGraphSonSupport()
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
                            .Remove(typeof(byte[])))));
        }
    }
}
