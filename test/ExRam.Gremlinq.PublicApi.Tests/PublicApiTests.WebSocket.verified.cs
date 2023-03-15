namespace ExRam.Gremlinq.Core
{
    public static class GremlinClientExtensions
    {
        public static Gremlin.Net.Driver.IGremlinClient ObserveResultStatusAttributes(this Gremlin.Net.Driver.IGremlinClient client, System.Action<Gremlin.Net.Driver.Messages.RequestMessage, System.Collections.Generic.IReadOnlyDictionary<string, object>> observer) { }
        public static Gremlin.Net.Driver.IGremlinClient TransformRequest(this Gremlin.Net.Driver.IGremlinClient client, System.Func<Gremlin.Net.Driver.Messages.RequestMessage, System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.RequestMessage>> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinClientFactory
    {
        public static readonly ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory Default;
        public static ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory Create(System.Func<Gremlin.Net.Driver.GremlinServer, Gremlin.Net.Driver.IMessageSerializer, Gremlin.Net.Driver.ConnectionPoolSettings, System.Action<System.Net.WebSockets.ClientWebSocketOptions>, string?, Gremlin.Net.Driver.IGremlinClient> factory) { }
    }
    public static class GremlinServerExtensions
    {
        public static Gremlin.Net.Driver.GremlinServer WithHost(this Gremlin.Net.Driver.GremlinServer server, string host) { }
        public static Gremlin.Net.Driver.GremlinServer WithPassword(this Gremlin.Net.Driver.GremlinServer server, string password) { }
        public static Gremlin.Net.Driver.GremlinServer WithPort(this Gremlin.Net.Driver.GremlinServer server, int port) { }
        public static Gremlin.Net.Driver.GremlinServer WithSslEnabled(this Gremlin.Net.Driver.GremlinServer server, bool sslEnabled) { }
        public static Gremlin.Net.Driver.GremlinServer WithUsername(this Gremlin.Net.Driver.GremlinServer server, string username) { }
    }
    public interface IGremlinClientFactory
    {
        Gremlin.Net.Driver.IGremlinClient Create(Gremlin.Net.Driver.GremlinServer gremlinServer, Gremlin.Net.Driver.IMessageSerializer messageSerializer, Gremlin.Net.Driver.ConnectionPoolSettings connectionPoolSettings, System.Action<System.Net.WebSockets.ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null);
    }
    public interface IWebSocketProviderConfigurator<out TConfigurator> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator>, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
        where out TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        TConfigurator ConfigureAlias(System.Func<string, string> transformation);
        TConfigurator ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory, ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory> transformation);
        TConfigurator ConfigureServer(System.Func<Gremlin.Net.Driver.GremlinServer, Gremlin.Net.Driver.GremlinServer> transformation);
    }
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator At<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> builder, string uri)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator At<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> configurator, System.Uri uri)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator AtLocalhost<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator AuthenticateBy<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> configurator, string username, string password)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator ConfigureClient<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> configurator, System.Func<Gremlin.Net.Driver.IGremlinClient, Gremlin.Net.Driver.IGremlinClient> transformation)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator ConfigureMessageSerializer<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> configurator, System.Func<Gremlin.Net.Driver.IMessageSerializer, Gremlin.Net.Driver.IMessageSerializer> transformation)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator SetAlias<TConfigurator>(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> configurator, string alias)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
    }
    public sealed class WebSocketProviderConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator>, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator>, ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator>
    {
        public WebSocketProviderConfigurator() { }
        public WebSocketProviderConfigurator(ExRam.Gremlinq.Core.GremlinqConfigurator gremlinqConfigurator, Gremlin.Net.Driver.GremlinServer gremlinServer, ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory clientFactory, string alias) { }
        public ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator ConfigureAlias(System.Func<string, string> transformation) { }
        public ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory, ExRam.Gremlinq.Providers.WebSocket.IGremlinClientFactory> transformation) { }
        public ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator ConfigureQuerySource(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.IGremlinQuerySource> transformation) { }
        public ExRam.Gremlinq.Providers.WebSocket.WebSocketProviderConfigurator ConfigureServer(System.Func<Gremlin.Net.Driver.GremlinServer, Gremlin.Net.Driver.GremlinServer> transformation) { }
        public ExRam.Gremlinq.Core.IGremlinQuerySource Transform(ExRam.Gremlinq.Core.IGremlinQuerySource source) { }
    }
}