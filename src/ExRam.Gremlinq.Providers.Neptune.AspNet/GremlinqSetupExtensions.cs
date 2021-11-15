using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Neptune;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class NeptuneConfiguratorTransformation : IProviderConfiguratorTransformation<INeptuneConfigurator>
        {
            private readonly IProviderConfiguration _configuration;

            public NeptuneConfiguratorTransformation(IProviderConfiguration configuration)
            {
                _configuration = configuration;
            }

            public INeptuneConfigurator Transform(INeptuneConfigurator configurator)
            {
                if (_configuration.GetSection("ElasticSearch") is { } elasticSearchSection)
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

                return configurator;
            }
        }

        public static GremlinqSetup UseNeptune(this GremlinqSetup setup, Action<ProviderSetup<INeptuneConfigurator>>? configuration = null)
        {
            return setup
                .UseProvider(
                    "Neptune",
                    (source, configuratorTransformation) => source.UseNeptune(configuratorTransformation),
                    configuration)
                .RegisterTypes(serviceCollection => serviceCollection
                    .TryAddSingleton<IProviderConfiguratorTransformation<INeptuneConfigurator>, NeptuneConfiguratorTransformation>());
        }

        public static GremlinqSetup UseNeptune<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<INeptuneConfigurator>>? configuration = null)
        {
            return setup
                .UseNeptune(configuration)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
