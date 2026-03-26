namespace ExRam.Gremlinq.Providers.Neptune.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> UseIAMAuthentication<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> UseIAMAuthentication<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder, Amazon.Runtime.Identity.IIdentityResolver<Amazon.Runtime.AWSCredentials> identityResolver, Amazon.Runtime.IClientConfig? clientConfig = null)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> UseIAMAuthenticationWithDefaultAWSCredentials<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder, Amazon.Runtime.IClientConfig? clientConfig = null)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator> UseNeptune<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup) { }
    }
}