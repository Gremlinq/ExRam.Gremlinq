using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Neptune;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<INeptuneConfigurator> UseNeptune<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .ConfigureBase()
                .UseProvider<INeptuneConfigurator>(source => source
                    .UseNeptune<TVertexBase, TEdgeBase>)
                .Configure((configurator, gremlinqSection) =>
                {
                    var providerSection = gremlinqSection
                        .GetSection("Neptune");

                    configurator = configurator
                        .ConfigureWebSocket(providerSection);

                    if (providerSection.GetSection("ElasticSearch") is { } elasticSearchSection)
                    {
                        if (bool.TryParse(elasticSearchSection["Enabled"], out var isEnabled) && isEnabled)
                        {
                            if (elasticSearchSection["EndPoint"] is { } endPoint && Uri.TryCreate(endPoint, UriKind.Absolute, out var uri))
                            {
                                var indexConfiguration = Enum.TryParse<NeptuneElasticSearchIndexConfiguration>(elasticSearchSection["IndexConfiguration"], true, out var outVar)
                                    ? outVar
                                    : NeptuneElasticSearchIndexConfiguration.Standard;

                                configurator = configurator
                                    .UseElasticSearch(uri, indexConfiguration);
                            }
                        }
                    }

                    return configurator;
                });
        }
    }
}
