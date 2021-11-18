namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TConfigurator> ConfigureWebSocket<TConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TConfigurator> setup)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
    }
    public static class WebSocketConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator ConfigureFrom(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator webSocketConfigurator, Microsoft.Extensions.Configuration.IConfiguration configuration) { }
    }
}