using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.Neptune;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseNeptune<TVertexBase, TEdgeBase>(this GremlinqSetup setup, Func<INeptuneConfigurator, IConfigurationSection, INeptuneConfigurator>? configuration = null)
        {
            return setup
                .UseProvider<INeptuneConfigurator>(
                    "Neptune",
                    (source, configuratorTransformation) => source
                        .UseNeptune<TVertexBase, TEdgeBase>(configuratorTransformation),
                    setup => setup
                        .Configure()
                        .Configure((configurator, providerSection) =>
                        {
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

                            return configuration?.Invoke(configurator, providerSection) ?? configurator;
                        }));
        }
    }
}
