namespace ExRam.Gremlinq.Providers.Core
{
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator At<TConfigurator>(this IProviderConfigurator<TConfigurator> builder, string uri)
            where TConfigurator : IProviderConfigurator<TConfigurator> => builder.At(new Uri(uri));

        public static TConfigurator AtLocalhost<TConfigurator>(this IProviderConfigurator<TConfigurator> builder)
            where TConfigurator : IProviderConfigurator<TConfigurator> => builder.At(new Uri("ws://localhost:8182"));

        public static TConfigurator At<TConfigurator>(this IProviderConfigurator<TConfigurator> configurator, Uri uri)
            where TConfigurator : IProviderConfigurator<TConfigurator> => configurator
                .ConfigureClientFactory(factory => factory
                    .ConfigureServer(server => server
                        .WithUri(uri)));

        public static TConfigurator AuthenticateBy<TConfigurator>(this IProviderConfigurator<TConfigurator> configurator, string username, string password)
            where TConfigurator : IProviderConfigurator<TConfigurator> => configurator
                .ConfigureClientFactory(factory => factory
                    .ConfigureServer(server => server
                        .WithUsername(username)
                        .WithPassword(password)));
    }
}
