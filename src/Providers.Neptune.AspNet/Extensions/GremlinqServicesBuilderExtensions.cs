using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.Core;

using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.Neptune.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        private sealed class UseIAMAuthenticationGremlinqConfiguratorTransformation<TConfigurator> : IGremlinqConfiguratorTransformation<TConfigurator>
           where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            private readonly IAWSSigner _signer;

            public UseIAMAuthenticationGremlinqConfiguratorTransformation(IAWSSigner signer)
            {
                _signer = signer;
            }

            public TConfigurator Transform(TConfigurator configurator) => ReferenceEquals(_signer, AWSSigner.Disabled)
                ? configurator
                : configurator.UseIAMAuthentication(_signer);
        }

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

                    if (providerSection["UseDFE"] is { } useDFEString && bool.TryParse(useDFEString, out var useDFE))
                    {
                        configurator = configurator
                            .ConfigureQuerySource(source => source
                                .UseDFE(useDFE));
                    }

                    return configurator;
                });
        }

        public static IGremlinqServicesBuilder<TConfigurator> UseIAMAuthentication<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            builder.Services
                .AddSingleton(ctx =>
                {
                    var gremlinqSection = ctx
                        .GetRequiredService<IGremlinqConfigurationSection>();

                    var iamSection = gremlinqSection
                        .GetSection("Neptune")
                        .GetSection("IAM");

                    if (bool.TryParse(iamSection["Disabled"], out var disabled) && disabled)
                        return AWSSigner.Disabled;

                    var signer = AWSSigner.EmptySigV4;

                    if (iamSection["Uri"] is { Length: > 0 } uri)
                        signer = signer.ConfigureUri(_ => new Uri(uri));
                    else if (gremlinqSection["Uri"] is { Length: > 0 } generalUri)
                        signer = signer.ConfigureUri(_ => new Uri(generalUri).EnsurePath());

                    if (iamSection["Region"] is { Length: > 0 } region)
                        signer = signer.ConfigureRegion(_ => region);

                    if (iamSection["AccessKeyId"] is { Length: > 0 } accessKeyId)
                        signer = signer.WithAccessKeyId(accessKeyId);

                    if (iamSection["SecretAccessKey"] is { Length: > 0 } accessKey)
                        signer = signer.WithSecretAccessKey(accessKey);

                    return signer;
                });

            return builder
                .Configure<UseIAMAuthenticationGremlinqConfiguratorTransformation<TConfigurator>>();
        }

        private static Uri EnsurePath(this Uri uri) => uri is { AbsolutePath: null or "" or "/" }
            ? new UriBuilder(uri) { Path = "gremlin" }.Uri
            : uri;
    }
}
