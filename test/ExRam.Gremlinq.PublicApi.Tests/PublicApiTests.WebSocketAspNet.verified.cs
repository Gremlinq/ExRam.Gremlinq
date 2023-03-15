namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public static class ProviderSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TConfigurator> ConfigureWebSocket<TConfigurator>(this ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<TConfigurator> setup)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}
namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator ConfigureFrom<TConfigurator>(this TConfigurator webSocketConfigurator, Microsoft.Extensions.Configuration.IConfiguration configuration)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}