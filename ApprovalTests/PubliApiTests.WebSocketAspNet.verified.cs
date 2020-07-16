[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup ConfigureWebSocket(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder> transformation) { }
    }
    public interface IWebSocketGremlinQueryExecutorBuilderTransformation
    {
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder Transform(ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder builder);
    }
    public static class WebSocketGremlinQueryExecutorBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder Configure(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder builder, Microsoft.Extensions.Configuration.IConfiguration configuration) { }
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder Transform(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder builder, System.Collections.Generic.IEnumerable<ExRam.Gremlinq.Core.AspNet.IWebSocketGremlinQueryExecutorBuilderTransformation> webSocketTransformations) { }
    }
}