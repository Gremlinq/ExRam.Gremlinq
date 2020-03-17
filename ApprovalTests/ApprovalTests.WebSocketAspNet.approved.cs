[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfigurationBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurationBuilder Configure(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurationBuilder builder, Microsoft.Extensions.Configuration.IConfiguration configuration) { }
    }
}