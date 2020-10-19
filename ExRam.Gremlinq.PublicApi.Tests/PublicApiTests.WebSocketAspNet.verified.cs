namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup ConfigureWebSocketBuilder(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder> transformation) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseWebSocket(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup) { }
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