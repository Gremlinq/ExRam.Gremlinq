using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class GremlinServerConfigurator : IGremlinServerConfigurator
        {
            private readonly WebSocketProviderConfigurator _baseConfigurator;

            public GremlinServerConfigurator() : this(new WebSocketProviderConfigurator())
            {
            }

            public GremlinServerConfigurator(WebSocketProviderConfigurator baseConfigurator)
            {
                _baseConfigurator = baseConfigurator;
            }

            public IGremlinServerConfigurator ConfigureAlias(Func<string, string> transformation) => new GremlinServerConfigurator(_baseConfigurator.ConfigureAlias(transformation));

            public IGremlinServerConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new GremlinServerConfigurator(_baseConfigurator.ConfigureClientFactory(transformation));

            public IGremlinServerConfigurator ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new GremlinServerConfigurator(_baseConfigurator.ConfigureServer(transformation));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _baseConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseGremlinServer(this IConfigurableGremlinQuerySource source, Func<IGremlinServerConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(new GremlinServerConfigurator())
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(graphFeatures => graphFeatures & ~(GraphFeatures.Transactions | GraphFeatures.ThreadedTransactions | GraphFeatures.ConcurrentAccess))
                            .ConfigureVertexFeatures(vertexFeatures => vertexFeatures & ~(VertexFeatures.Upsert | VertexFeatures.CustomIds))
                            .ConfigureVertexPropertyFeatures(vPropertiesFeatures => vPropertiesFeatures & ~(VertexPropertyFeatures.CustomIds))
                            .ConfigureEdgeFeatures(edgeProperties => edgeProperties & ~(EdgeFeatures.Upsert | EdgeFeatures.CustomIds)))
                        .UseGraphSon3()
                        .UseNewtonsoftJson()));
        }
    }
}
