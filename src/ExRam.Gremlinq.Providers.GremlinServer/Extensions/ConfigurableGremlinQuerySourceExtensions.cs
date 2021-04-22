using System;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class GremlinServerConfigurator : IGremlinServerConfigurator
        {
            private readonly IWebSocketConfigurator _baseConfigurator;

            public GremlinServerConfigurator(IWebSocketConfigurator baseConfigurator)
            {
                _baseConfigurator = baseConfigurator;
            }

            public IGremlinServerConfigurator At(Uri uri) => new GremlinServerConfigurator(_baseConfigurator.At(uri));

            public IGremlinServerConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation) => new GremlinServerConfigurator(transformation(_baseConfigurator));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _baseConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseGremlinServer(this IConfigurableGremlinQuerySource source, Func<IGremlinServerConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return source
                .UseWebSocket(configurator => configuratorTransformation(new GremlinServerConfigurator(configurator)))
                .ConfigureEnvironment(environment => environment
                    .ConfigureFeatureSet(featureSet => featureSet
                        .ConfigureGraphFeatures(graphFeatures => graphFeatures & ~(GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.ConcurrentAccess))
                        .ConfigureVertexFeatures(vertexFeatures => vertexFeatures & ~(VertexFeatures.Upsert | VertexFeatures.CustomIds))
                        .ConfigureVertexPropertyFeatures(vPropertiesFeatures => vPropertiesFeatures & ~(VertexPropertyFeatures.CustomIds))
                        .ConfigureEdgeFeatures(edgeProperties => edgeProperties & ~(EdgeFeatures.Upsert | EdgeFeatures.CustomIds))));
        }
    }
}
