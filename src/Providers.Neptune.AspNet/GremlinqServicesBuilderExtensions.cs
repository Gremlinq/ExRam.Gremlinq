using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.Neptune;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqProviderServicesBuilder<INeptuneConfigurator> UseNeptune<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            return setup
                .UseProvider<INeptuneConfigurator>(
                    (source, configurationContinuation) => source.UseNeptune<TVertexBase, TEdgeBase>(configurationContinuation))
                .FromSection("Neptune")
                .Configure((configurator, section) =>
                {
                    configurator = configurator
                        .ConfigureBase(section.GremlinqSection)
                        .ConfigureWebSocket(section);

                    if (section.GetSection("ElasticSearch") is { } elasticSearchSection)
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
