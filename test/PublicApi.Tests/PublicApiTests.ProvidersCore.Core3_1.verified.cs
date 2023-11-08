namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClient ObserveResultStatusAttributes(this ExRam.Gremlinq.Providers.Core.IGremlinqClient client, System.Action<Gremlin.Net.Driver.Messages.RequestMessage, System.Collections.Generic.IReadOnlyDictionary<string, object>> observer) { }
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClient TransformRequest(this ExRam.Gremlinq.Providers.Core.IGremlinqClient client, System.Func<Gremlin.Net.Driver.Messages.RequestMessage, System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.RequestMessage>> transformation) { }
    }
    public static class GremlinServerExtensions
    {
        public static Gremlin.Net.Driver.GremlinServer WithHost(this Gremlin.Net.Driver.GremlinServer server, string host) { }
        public static Gremlin.Net.Driver.GremlinServer WithPassword(this Gremlin.Net.Driver.GremlinServer server, string password) { }
        public static Gremlin.Net.Driver.GremlinServer WithPort(this Gremlin.Net.Driver.GremlinServer server, int port) { }
        public static Gremlin.Net.Driver.GremlinServer WithSslEnabled(this Gremlin.Net.Driver.GremlinServer server, bool sslEnabled) { }
        public static Gremlin.Net.Driver.GremlinServer WithUsername(this Gremlin.Net.Driver.GremlinServer server, string username) { }
    }
    public static class GremlinqClientFactory
    {
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory ConfigureClient(this ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory clientFactory, System.Func<ExRam.Gremlinq.Providers.Core.IGremlinqClient, ExRam.Gremlinq.Providers.Core.IGremlinqClient> clientTransformation) { }
    }
    public interface IGremlinqClient : System.IDisposable
    {
        System.Collections.Generic.IAsyncEnumerable<Gremlin.Net.Driver.Messages.ResponseMessage<T>> SubmitAsync<T>(Gremlin.Net.Driver.Messages.RequestMessage message);
    }
    public interface IGremlinqClientFactory
    {
        ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory ConfigureConnectionPool(System.Action<Gremlin.Net.Driver.ConnectionPoolSettings> configuration);
        ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory ConfigureServer(System.Func<Gremlin.Net.Driver.GremlinServer, Gremlin.Net.Driver.GremlinServer> transformation);
        ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory ConfigureWebSocketOptions(System.Action<System.Net.WebSockets.ClientWebSocketOptions> configuration);
        ExRam.Gremlinq.Providers.Core.IGremlinqClient Create(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IWebSocketProviderConfigurator<out TSelf> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
        where out TSelf : ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
    {
        TSelf ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory, ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory> transformation);
    }
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator At<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> builder, string uri)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator At<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> configurator, System.Uri uri)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator AtLocalhost<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator AuthenticateBy<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> configurator, string username, string password)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
    }
    public sealed class WebSocketProviderConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator>, ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator>
    {
        public static readonly ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator Default;
        public ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory, ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory> transformation) { }
        public ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator ConfigureQuerySource(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.IGremlinQuerySource> transformation) { }
        public ExRam.Gremlinq.Core.IGremlinQuerySource Transform(ExRam.Gremlinq.Core.IGremlinQuerySource source) { }
    }
}