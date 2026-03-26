using Amazon.Runtime;
using Amazon.Runtime.Credentials;
using Amazon.Runtime.Identity;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.Neptune.AspNet
{
    /// <summary>
    /// Provides extension methods for <see cref="IGremlinqServicesBuilder"/> to register the AWS Neptune provider with ASP.NET Core dependency injection.
    /// </summary>
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

        /// <summary>
        /// Registers the AWS Neptune Gremlin provider and configures it from the application's configuration section.
        /// </summary>
        /// <typeparam name="TVertexBase">The base type for all vertex entities.</typeparam>
        /// <typeparam name="TEdgeBase">The base type for all edge entities.</typeparam>
        /// <param name="setup">The services builder to configure.</param>
        public static IGremlinqServicesBuilder<INeptuneConfigurator> UseNeptune<TVertexBase, TEdgeBase>(this IGremlinqServicesBuilder setup)
        {
            ArgumentNullException.ThrowIfNull(setup);

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

        /// <summary>
        /// Configures the provider to use AWS IAM authentication, reading credentials from the application's configuration section.
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="builder">The services builder to configure.</param>
        public static IGremlinqServicesBuilder<TConfigurator> UseIAMAuthentication<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .UseIAMAuthentication(_ => _);
        }

        /// <summary>
        /// Configures the provider to use AWS IAM authentication, reading URI and region
        /// from the application's configuration section and resolving credentials from the
        /// specified <see cref="IIdentityResolver{T}"/> for <see cref="AWSCredentials"/>.
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="builder">The services builder to configure.</param>
        /// <param name="identityResolver">The identity resolver to use for obtaining AWS credentials.</param>
        /// <param name="clientConfig">Optional AWS client configuration.</param>
        public static IGremlinqServicesBuilder<TConfigurator> UseIAMAuthentication<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder, IIdentityResolver<AWSCredentials> identityResolver, IClientConfig? clientConfig = null)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(identityResolver);

            return builder
                .UseIAMAuthentication(_ => _
                    .WithCredentials(identityResolver, clientConfig));
        }

        /// <summary>
        /// Configures the provider to use AWS IAM authentication, reading URI and region
        /// from the application's configuration section and resolving credentials from a
        /// <see cref="DefaultAWSCredentialsIdentityResolver"/> for <see cref="AWSCredentials"/>.
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="builder">The services builder to configure.</param>
        /// <param name="clientConfig">Optional AWS client configuration.</param>
        public static IGremlinqServicesBuilder<TConfigurator> UseIAMAuthenticationWithDefaultAWSCredentials<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder, IClientConfig? clientConfig = null)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .UseIAMAuthentication(new DefaultAWSCredentialsIdentityResolver(), clientConfig);
        }

        private static IGremlinqServicesBuilder<TConfigurator> UseIAMAuthentication<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder, Func<ISigV4AWSSigner, ISigV4AWSSigner> signerTransformation)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services
                .AddSingleton<IAWSSigner>(ctx =>
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
                        signer = signer.WithUri(new Uri(uri));
                    else if (gremlinqSection["Uri"] is { Length: > 0 } generalUri)
                        signer = signer.WithUri(new Uri(generalUri));

                    if (iamSection["Region"] is { Length: > 0 } region)
                        signer = signer.WithRegion(region);

                    if (iamSection["AccessKeyId"] is { Length: > 0 } accessKeyId)
                        signer = signer.WithAccessKeyId(accessKeyId);

                    if (iamSection["SecretAccessKey"] is { Length: > 0 } accessKey)
                        signer = signer.WithSecretAccessKey(accessKey);

                    return signerTransformation(signer);
                });

            return builder
                .Configure<UseIAMAuthenticationGremlinqConfiguratorTransformation<TConfigurator>>();
        }
    }
}
