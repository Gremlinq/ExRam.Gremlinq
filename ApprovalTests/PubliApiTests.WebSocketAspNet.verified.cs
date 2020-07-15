[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup ConfigureWebSocket(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder> transformation) { }
    }
    public interface IWebSocketGremlinQueryEnvironmentBuilderTransformation
    {
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder Transform(ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder builder);
    }
    public static class WebSocketGremlinQueryEnvironmentBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder Configure(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder builder, Microsoft.Extensions.Configuration.IConfiguration configuration, System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.AspNet.IWebSocketGremlinQueryEnvironmentBuilderTransformation> webSocketTransformations) { }
    }
}