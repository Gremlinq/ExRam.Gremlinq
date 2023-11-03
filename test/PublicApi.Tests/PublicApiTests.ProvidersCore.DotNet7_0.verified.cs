namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientExtensions
    {
        public static Gremlin.Net.Driver.IGremlinClient ObserveResultStatusAttributes(this Gremlin.Net.Driver.IGremlinClient client, System.Action<Gremlin.Net.Driver.Messages.RequestMessage, System.Collections.Generic.IReadOnlyDictionary<string, object>> observer) { }
        public static Gremlin.Net.Driver.IGremlinClient TransformRequest(this Gremlin.Net.Driver.IGremlinClient client, System.Func<Gremlin.Net.Driver.Messages.RequestMessage, System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.RequestMessage>> transformation) { }
    }
    public static class GremlinClientFactory
    {
        public static ExRam.Gremlinq.Providers.Core.IGremlinClientFactory ConfigureClient(this ExRam.Gremlinq.Providers.Core.IGremlinClientFactory clientFactory, System.Func<Gremlin.Net.Driver.IGremlinClient, Gremlin.Net.Driver.IGremlinClient> clientTransformation) { }
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
        ExRam.Gremlinq.Providers.Core.IGremlinClientFactory ConfigureConnectionPool(System.Action<Gremlin.Net.Driver.ConnectionPoolSettings> configuration);
        ExRam.Gremlinq.Providers.Core.IGremlinClientFactory ConfigureServer(System.Func<Gremlin.Net.Driver.GremlinServer, Gremlin.Net.Driver.GremlinServer> transformation);
        ExRam.Gremlinq.Providers.Core.IGremlinClientFactory ConfigureWebSocketOptions(System.Action<System.Net.WebSockets.ClientWebSocketOptions> configuration);
        Gremlin.Net.Driver.IGremlinClient Create(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IWebSocketProviderConfigurator<out TSelf> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
        where out TSelf : ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
    {
        TSelf ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.Core.IGremlinClientFactory, ExRam.Gremlinq.Providers.Core.IGremlinClientFactory> transformation);
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
        public ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator ConfigureClientFactory(System.Func<ExRam.Gremlinq.Providers.Core.IGremlinClientFactory, ExRam.Gremlinq.Providers.Core.IGremlinClientFactory> transformation) { }
        public ExRam.Gremlinq.Providers.Core.WebSocketProviderConfigurator ConfigureQuerySource(System.Func<ExRam.Gremlinq.Core.IGremlinQuerySource, ExRam.Gremlinq.Core.IGremlinQuerySource> transformation) { }
        public ExRam.Gremlinq.Core.IGremlinQuerySource Transform(ExRam.Gremlinq.Core.IGremlinQuerySource source) { }
    }
}