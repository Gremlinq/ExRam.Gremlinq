using System;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseNeptune(this GremlinqSetup setup, Action<ProviderSetup<INeptuneConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "Neptune",
                    (source, configuratorTransformation) => source
                        .UseNeptune(configuratorTransformation),
                    setup => setup
                        .ConfigureWebSocket()
                        .Configure((configurator, providerSection) =>
                        {
                            if (providerSection.GetSection("IAM") is { } iamSection)
                            {
                                if (iamSection["AccessKeyId"] is { } accessKeyId)
                                {
                                    configurator
                                        .ConfigureWebSocket(configurator => configurator
                                            .ConfigureServer(server => server
                                                .WithUsername(accessKeyId)));
                                }

                                if (iamSection["SecretAccessKey"] is { } secretAccessKey)
                                {
                                    configurator
                                        .ConfigureWebSocket(configurator => configurator
                                            .ConfigureServer(server => server
                                                .WithPassword(secretAccessKey)));
                                }
                            }

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
                        }),
                    extraSetupAction);
        }

        public static GremlinqSetup UseNeptune<TVertex, TEdge>(this GremlinqSetup setup, Action<ProviderSetup<INeptuneConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseNeptune(extraSetupAction)
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())));
        }
    }
}
