using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.Neptune;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ProviderSetup<INeptuneConfigurator> UseNeptune<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<INeptuneConfigurator, IProviderConfigurationSection, INeptuneConfigurator>? configuratorTransformation = null)
        {
            return setup
                .UseProvider<INeptuneConfigurator>(
                    "Neptune",
                    (source, section) => source
                        .UseNeptune<TVertexBase, TEdgeBase>(configurator =>
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

                            return configuratorTransformation?.Invoke(configurator, section) ?? configurator;
                        }));
        }
    }
}
