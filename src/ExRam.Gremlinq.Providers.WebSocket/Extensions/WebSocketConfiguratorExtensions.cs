using System;

using Gremlin.Net.Driver;

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

        public static IWebSocketConfigurator ConfigureGremlinClient(this IWebSocketConfigurator configurator, Func<IGremlinClient, IGremlinClient> transformation)
        {
            return configurator
                .ConfigureGremlinClientFactory(factory => GremlinClientFactory
                    .Create((server, serializer, poolSettings, optionsTransformation, sessionId) =>
                    {
                        return transformation(factory.Create(server, serializer, poolSettings, optionsTransformation, sessionId));
                    }));
        }

        public static IWebSocketConfigurator ConfigureMessageSerializer(this IWebSocketConfigurator configurator, Func<IMessageSerializer, IMessageSerializer> transformation)
        {
            return configurator
                .ConfigureGremlinClientFactory(factory => GremlinClientFactory
                    .Create((server, maybeSerializer, poolSettings, optionsTransformation, sessionId) =>
                    {
                        return factory.Create(server, maybeSerializer is { } serializer ? transformation(serializer) : maybeSerializer, poolSettings, optionsTransformation, sessionId);
                    }));
        }

    }
}
