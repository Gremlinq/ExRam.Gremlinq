namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseWebSocket(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
    public static class GremlinClientExtensions
    {
        public static Gremlin.Net.Driver.IGremlinClient ObserveResultStatusAttributes(this Gremlin.Net.Driver.IGremlinClient client, System.Action<Gremlin.Net.Driver.Messages.RequestMessage, System.Collections.Generic.IReadOnlyDictionary<string, object>> observer) { }
        public static Gremlin.Net.Driver.IGremlinClient TransformRequest(this Gremlin.Net.Driver.IGremlinClient client, System.Func<Gremlin.Net.Driver.Messages.RequestMessage, System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.RequestMessage>> transformation) { }
    }
    public abstract class JsonNetMessageSerializer : Gremlin.Net.Driver.IMessageSerializer
    {
        public static readonly Gremlin.Net.Driver.IMessageSerializer GraphSON2;
        public static readonly Gremlin.Net.Driver.IMessageSerializer GraphSON3;
        protected JsonNetMessageSerializer(string mimeType, Gremlin.Net.Structure.IO.GraphSON.GraphSONWriter graphSonWriter) { }
        public System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.ResponseMessage<System.Collections.Generic.List<object>>> DeserializeMessageAsync(byte[] message) { }
        public System.Threading.Tasks.Task<byte[]> SerializeMessageAsync(Gremlin.Net.Driver.Messages.RequestMessage requestMessage) { }
    }
}
namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation
    {
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator At(System.Uri uri);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator AuthenticateBy(string username, string password);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator ConfigureConnectionPool(System.Action<Gremlin.Net.Driver.ConnectionPoolSettings> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator ConfigureGremlinClient(System.Func<Gremlin.Net.Driver.IGremlinClient, Gremlin.Net.Driver.IGremlinClient> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator ConfigureMessageSerializer(System.Func<Gremlin.Net.Driver.IMessageSerializer, Gremlin.Net.Driver.IMessageSerializer> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator SetAlias(string alias);
    }
    public interface IWebSocketProviderConfigurator<out TConfigurator> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
        where out TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator>
    {
        TConfigurator ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator> transformation);
    }
    public static class WebSocketConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator At(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator builder, string uri) { }
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator AtLocalhost(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator builder) { }
    }
    public static class WebSocketGremlinqOptions
    {
        public static ExRam.Gremlinq.Core.GremlinqOption<Newtonsoft.Json.Formatting> QueryLogFormatting;
        public static ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.Serialization.GroovyFormatting> QueryLogGroovyFormatting;
        public static ExRam.Gremlinq.Core.GremlinqOption<Microsoft.Extensions.Logging.LogLevel> QueryLogLogLevel;
        public static ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.QueryLogVerbosity> QueryLogVerbosity;
    }
    public static class WebSocketProviderConfiguratorExtensions
    {
        public static TConfigurator AtLocalhost<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
        public static TConfigurator AtLocalhost<TConfigurator>(this TConfigurator configurator, System.Uri uri)
            where TConfigurator : ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}