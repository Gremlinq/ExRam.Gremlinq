using System;
using Gremlin.Net.Driver;
using _GremlinServer = Gremlin.Net.Driver.GremlinServer;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketConfiguratorExtensions
    {
        public static IWebSocketConfigurator At(this IWebSocketConfigurator builder, string uri)
        {
            return builder.At(new Uri(uri));
        }

        public static IWebSocketConfigurator AtLocalhost(this IWebSocketConfigurator builder)
        {
            return builder.At(new Uri("ws://localhost:8182"));
        }

        public static IWebSocketConfigurator ConfigureClient(this IWebSocketConfigurator configurator, Func<IGremlinClient, IGremlinClient> transformation)
        {
            return configurator
                .ConfigureClientFactory(factory => GremlinClientFactory
                    .Create((server, serializer, poolSettings, optionsTransformation, sessionId) => transformation(factory.Create(server, serializer, poolSettings, optionsTransformation, sessionId))));
        }

        public static IWebSocketConfigurator ConfigureMessageSerializer(this IWebSocketConfigurator configurator, Func<IMessageSerializer, IMessageSerializer> transformation)
        {
            return configurator
                .ConfigureClientFactory(factory => GremlinClientFactory
                    .Create((server, maybeSerializer, poolSettings, optionsTransformation, sessionId) => factory.Create(server, maybeSerializer is { } serializer ? transformation(serializer) : maybeSerializer, poolSettings, optionsTransformation, sessionId)));
        }

        public static IWebSocketConfigurator SetAlias(this IWebSocketConfigurator configurator, string alias) => configurator.ConfigureAlias(_ => alias);

        public static IWebSocketConfigurator At(this IWebSocketConfigurator configurator, Uri uri) => configurator
            .ConfigureServer(server => server
                .WithUri(uri));

        public static IWebSocketConfigurator AuthenticateBy(this IWebSocketConfigurator configurator, string username, string password) => configurator
            .ConfigureServer(server => server
                .WithUsername(username)
                .WithPassword(password));
    }
}
