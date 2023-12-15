namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct GraphSon2MessageBuffer : System.Buffers.IMemoryOwner<byte>, System.IDisposable
    {
        public GraphSon2MessageBuffer(System.Buffers.IMemoryOwner<byte> owner) { }
        public System.Memory<byte> Memory { get; }
        public void Dispose() { }
    }
    public readonly struct GraphSon3MessageBuffer : System.Buffers.IMemoryOwner<byte>, System.IDisposable
    {
        public GraphSon3MessageBuffer(System.Buffers.IMemoryOwner<byte> owner) { }
        public System.Memory<byte> Memory { get; }
        public void Dispose() { }
    }
    public static class GremlinqClientExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClient ObserveResultStatusAttributes(this ExRam.Gremlinq.Providers.Core.IGremlinqClient client, System.Action<Gremlin.Net.Driver.Messages.RequestMessage, System.Collections.Generic.IReadOnlyDictionary<string, object>> observer) { }
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClient Throttle(this ExRam.Gremlinq.Providers.Core.IGremlinqClient client, int maxConcurrency) { }
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClient TransformRequest(this ExRam.Gremlinq.Providers.Core.IGremlinqClient client, System.Func<Gremlin.Net.Driver.Messages.RequestMessage, System.Threading.CancellationToken, System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.RequestMessage>> transformation) { }
    }
    public static class GremlinqClientFactory
    {
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory ConfigureClient(this ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory clientFactory, System.Func<ExRam.Gremlinq.Providers.Core.IGremlinqClient, ExRam.Gremlinq.Providers.Core.IGremlinqClient> clientTransformation) { }
        public static ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory Log(this ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory clientFactory) { }
        public static ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<TBaseFactory> Pool<TBaseFactory>(this TBaseFactory baseFactory)
            where TBaseFactory : ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory { }
        public static ExRam.Gremlinq.Core.Execution.IGremlinQueryExecutor ToExecutor(this ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory clientFactory) { }
    }
    public interface IGremlinqClient : System.IDisposable
    {
        System.Collections.Generic.IAsyncEnumerable<Gremlin.Net.Driver.Messages.ResponseMessage<T>> SubmitAsync<T>(Gremlin.Net.Driver.Messages.RequestMessage message);
    }
    public interface IGremlinqClientFactory
    {
        ExRam.Gremlinq.Providers.Core.IGremlinqClient Create(ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment);
    }
    public interface IMessageBufferFactory<TBuffer>
        where TBuffer : System.Buffers.IMemoryOwner<byte>
    {
        string MimeType { get; }
        TBuffer Create(Gremlin.Net.Driver.Messages.RequestMessage message);
        TBuffer Create(System.Buffers.IMemoryOwner<byte> message);
    }
    public interface IPoolGremlinqClientFactory<TBaseFactory> : ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory
        where TBaseFactory : ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory
    {
        ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(System.Func<TBaseFactory, TBaseFactory> transformation);
        ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection);
        ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize);
    }
    public interface IProviderConfigurator<out TSelf, TClientFactory> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
        where out TSelf : ExRam.Gremlinq.Core.IGremlinqConfigurator<TSelf>
        where TClientFactory : ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory
    {
        TSelf ConfigureClientFactory(System.Func<TClientFactory, TClientFactory> transformation);
    }
    public interface IWebSocketGremlinqClientFactory : ExRam.Gremlinq.Providers.Core.IGremlinqClientFactory
    {
        ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory ConfigureAuthentication(System.Func<System.Func<System.Collections.Generic.IReadOnlyDictionary<string, object>, Gremlin.Net.Driver.Messages.RequestMessage>, System.Func<System.Collections.Generic.IReadOnlyDictionary<string, object>, Gremlin.Net.Driver.Messages.RequestMessage>> transformation);
        ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory ConfigureOptions(System.Action<System.Net.WebSockets.ClientWebSocketOptions> configuration);
        ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory ConfigureUri(System.Func<System.Uri, System.Uri> transformation);
        ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory WithMessageBufferFactory<TBuffer>(ExRam.Gremlinq.Providers.Core.IMessageBufferFactory<TBuffer> factory)
            where TBuffer : System.Buffers.IMemoryOwner<byte>;
    }
    public static class MessageBufferFactory
    {
        public static readonly ExRam.Gremlinq.Providers.Core.IMessageBufferFactory<ExRam.Gremlinq.Providers.Core.GraphSon2MessageBuffer> GraphSon2;
        public static readonly ExRam.Gremlinq.Providers.Core.IMessageBufferFactory<ExRam.Gremlinq.Providers.Core.GraphSon3MessageBuffer> GraphSon3;
    }
    public static class UriExtensions
    {
        public static System.Uri EnsurePath(this System.Uri uri) { }
    }
    public static class WebSocketConfiguratorExtensions
    {
        public static TConfigurator At<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> builder, string uri)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator At<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> configurator, System.Uri uri)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator AtLocalhost<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator AuthenticateBy<TConfigurator>(this ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> configurator, string username, string password)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
    }
    public static class WebSocketGremlinqClientFactory
    {
        public static readonly ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory LocalHost;
        public static ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory WithPlainCredentials(this ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory factory, string username, string password) { }
    }
}