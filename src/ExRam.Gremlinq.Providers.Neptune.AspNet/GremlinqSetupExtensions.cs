using System;

using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Neptune;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseNeptune(this GremlinqSetup setup, Func<INeptuneConfigurator, IConfiguration, INeptuneConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseProvider<INeptuneConfigurator>(
                    "Neptune",
                    (e, f) => e.UseNeptune(f),
                    (configurator, configuration) =>
                    {
                        if (configuration.GetSection("ElasticSearch") is { } elasticSearchSection)
                        {
                            if (bool.TryParse(elasticSearchSection["Enabled"], out var isEnabled) && isEnabled && elasticSearchSection["EndPoint"] is { } endPoint)
                            {
                                var indexConfiguration = Enum.TryParse<NeptuneElasticSearchIndexConfiguration>(elasticSearchSection["IndexConfiguration"], true, out var outVar)
                                    ? outVar
                                    : NeptuneElasticSearchIndexConfiguration.Standard;

                                configurator = configurator
                                    .UseElasticSearch(new Uri(endPoint), indexConfiguration);
                            }
                        }

                        return extraConfiguration?.Invoke(configurator, configuration) ?? configurator;
                    });
        }

        public static GremlinqSetup UseNeptune<TVertex, TEdge>(this GremlinqSetup setup, Func<INeptuneConfigurator, IConfiguration, INeptuneConfigurator>? extraConfiguration = null)
        {
            return setup
                .UseNeptune(extraConfiguration)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
