namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator ConfigureFrom<TConfigurator>(this TConfigurator webSocketConfigurator, Microsoft.Extensions.Configuration.IConfiguration configuration)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}