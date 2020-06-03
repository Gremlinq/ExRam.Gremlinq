[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core.AspNet
{
    public static class WebSocketConfigurationBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder Configure(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder builder, Microsoft.Extensions.Configuration.IConfiguration configuration) { }
    }
}