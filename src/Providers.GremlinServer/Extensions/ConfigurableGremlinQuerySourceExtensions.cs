using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class GremlinServerConfigurator : IGremlinServerConfigurator
        {
            public static readonly GremlinServerConfigurator Default = new(WebSocketGremlinqClientFactory.LocalHost.Pool(), _ => _);

            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _querySourceTransformation;
            private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> _clientFactory;

            private GremlinServerConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory, Func<IGremlinQuerySource, IGremlinQuerySource> querySourceTransformation)
            {
                _clientFactory = clientFactory;
                _querySourceTransformation = querySourceTransformation;
            }

            public IGremlinServerConfigurator ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> transformation) => new GremlinServerConfigurator(
                transformation(_clientFactory),
                _querySourceTransformation);

            public IGremlinServerConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new GremlinServerConfigurator(
                _clientFactory,
                _ => transformation(_querySourceTransformation(_)));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _querySourceTransformation
                .Invoke(source
                    .ConfigureEnvironment(environment => environment
                        .UseExecutor(_clientFactory
                            .Log()
                            .ToExecutor())));
        }

        public static IGremlinQuerySource UseGremlinServer<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<IGremlinServerConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(GremlinServerConfigurator.Default)
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .UseModel(GraphModel
                            .FromBaseTypes<TVertexBase, TEdgeBase>())
                        .ConfigureOptions(options => options
                            .SetValue(GremlinqOption.WorkaroundRangeInconsistencies, true))
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(graphFeatures => graphFeatures & ~(GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.ConcurrentAccess))
                            .ConfigureVertexFeatures(vertexFeatures => vertexFeatures & ~(VertexFeatures.Upsert | VertexFeatures.CustomIds))
                            .ConfigureVertexPropertyFeatures(vPropertiesFeatures => vPropertiesFeatures & ~(VertexPropertyFeatures.CustomIds))
                            .ConfigureEdgeFeatures(edgeProperties => edgeProperties & ~(EdgeFeatures.Upsert | EdgeFeatures.CustomIds)))));
        }
    }
}
