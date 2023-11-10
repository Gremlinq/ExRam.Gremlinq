namespace ExRam.Gremlinq.Providers.Core
{
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator At<TConfigurator, TClientFactory>(this IProviderConfigurator<TConfigurator, TClientFactory> builder, string uri)
            where TConfigurator : IProviderConfigurator<TConfigurator, TClientFactory>
            where TClientFactory : IGremlinqClientFactory  => builder.At(new Uri(uri));

        public static TConfigurator AtLocalhost<TConfigurator, TClientFactory>(this IProviderConfigurator<TConfigurator, TClientFactory> builder)
            where TConfigurator : IProviderConfigurator<TConfigurator, TClientFactory>
            where TClientFactory : IGremlinqClientFactory => builder.At(new Uri("ws://localhost:8182"));

        public static TConfigurator At<TConfigurator, TClientFactory>(this IProviderConfigurator<TConfigurator, TClientFactory> configurator, Uri uri)
            where TConfigurator : IProviderConfigurator<TConfigurator, TClientFactory>
            where TClientFactory : IGremlinqClientFactory => configurator
                .ConfigureClientFactory(factory => factory
                    .ConfigureServer(server => server
                        .WithUri(uri)));

        public static TConfigurator AuthenticateBy<TConfigurator, TClientFactory>(this IProviderConfigurator<TConfigurator, TClientFactory> configurator, string username, string password)
            where TConfigurator : IProviderConfigurator<TConfigurator, TClientFactory>
            where TClientFactory : IGremlinqClientFactory => configurator
                .ConfigureClientFactory(factory => factory
                    .ConfigureServer(server => server
                        .WithUsername(username)
                        .WithPassword(password)));
    }
}
