using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class GremlinServerConfigurator : IGremlinServerConfigurator
        {
            public static readonly GremlinServerConfigurator Default = new(ProviderConfigurator.Default);

            private readonly ProviderConfigurator _webSocketConfigurator;

            private GremlinServerConfigurator(ProviderConfigurator webSocketConfigurator)
            {
                _webSocketConfigurator = webSocketConfigurator;
            }

            public IGremlinServerConfigurator ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> transformation) => new GremlinServerConfigurator(_webSocketConfigurator.ConfigureClientFactory(transformation));

            public IGremlinServerConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new GremlinServerConfigurator(_webSocketConfigurator.ConfigureQuerySource(transformation));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _webSocketConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseGremlinServer<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<IGremlinServerConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(GremlinServerConfigurator.Default)
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .UseModel(GraphModel
                            .FromBaseTypes<TVertexBase, TEdgeBase>())
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(graphFeatures => graphFeatures & ~(GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.ConcurrentAccess))
                            .ConfigureVertexFeatures(vertexFeatures => vertexFeatures & ~(VertexFeatures.Upsert | VertexFeatures.CustomIds))
                            .ConfigureVertexPropertyFeatures(vPropertiesFeatures => vPropertiesFeatures & ~(VertexPropertyFeatures.CustomIds))
                            .ConfigureEdgeFeatures(edgeProperties => edgeProperties & ~(EdgeFeatures.Upsert | EdgeFeatures.CustomIds)))
                        .UseGraphSon3()));
        }
    }
}
