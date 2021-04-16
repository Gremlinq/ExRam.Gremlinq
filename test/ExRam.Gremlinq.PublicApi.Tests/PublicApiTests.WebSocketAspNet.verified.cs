namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseWebSocket(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup) { }
    }
    public static class WebSocketConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator Configure(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator configurator, Microsoft.Extensions.Configuration.IConfiguration configuration) { }
    }
}