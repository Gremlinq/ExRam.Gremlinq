namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// Provides extension methods for <see cref="IProviderConfigurator{TSelf, TClientFactory}"/> with WebSocket-based pool client factories.
    /// </summary>
    public static class ProviderConfiguratorExtensions
    {
        /// <summary>
        /// Configures the provider to connect to the specified URI.
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="builder">The configurator to configure.</param>
        /// <param name="uri">The URI string of the Gremlin server.</param>
        public static TConfigurator At<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> builder, string uri)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(uri);

            return builder.At(new Uri(uri));
        }

        /// <summary>
        /// Configures the provider to connect to the default localhost URI (<c>ws://localhost:8182</c>).
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="builder">The configurator to configure.</param>
        public static TConfigurator AtLocalhost<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> builder)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder.At(new Uri("ws://localhost:8182"));
        }

        /// <summary>
        /// Configures the provider to connect to the specified URI.
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="configurator">The configurator to configure.</param>
        /// <param name="uri">The URI of the Gremlin server.</param>
        public static TConfigurator At<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, Uri uri)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(uri);

            return configurator
                .ConfigureClientFactory(factory => factory
                    .ConfigureBaseFactory(factory => factory
                        .ConfigureUri(_ => uri)));
        }

        /// <summary>
        /// Configures the provider to authenticate with the specified username and password.
        /// </summary>
        /// <typeparam name="TConfigurator">The concrete configurator type.</typeparam>
        /// <param name="configurator">The configurator to configure.</param>
        /// <param name="username">The authentication username.</param>
        /// <param name="password">The authentication password.</param>
        public static TConfigurator AuthenticateBy<TConfigurator>(this IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> configurator, string username, string password)
            where TConfigurator : IProviderConfigurator<TConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>
        {
            ArgumentNullException.ThrowIfNull(configurator);
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            return configurator
                .ConfigureClientFactory(factory => factory
                    .ConfigureBaseFactory(factory => factory
                        .WithPlainCredentials(username, password)));
        }
    }
}
