namespace ExRam.Gremlinq.Core
{
    public static class GremlinClientExtensions
    {
        public static Gremlin.Net.Driver.IGremlinClient ObserveResultStatusAttributes(this Gremlin.Net.Driver.IGremlinClient client, System.Action<Gremlin.Net.Driver.Messages.RequestMessage, System.Collections.Generic.IReadOnlyDictionary<string, object>> observer) { }
        public static Gremlin.Net.Driver.IGremlinClient TransformRequest(this Gremlin.Net.Driver.IGremlinClient client, System.Func<Gremlin.Net.Driver.Messages.RequestMessage, System.Threading.Tasks.Task<Gremlin.Net.Driver.Messages.RequestMessage>> transformation) { }
    }
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseWebSocket(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation> builderTransformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : ExRam.Gremlinq.Core.IGremlinQueryEnvironmentTransformation
    {
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator AddGraphSONDeserializer(string typename, Gremlin.Net.Structure.IO.GraphSON.IGraphSONDeserializer serializer);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator AddGraphSONSerializer(System.Type type, Gremlin.Net.Structure.IO.GraphSON.IGraphSONSerializer serializer);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator At(System.Uri uri);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator AuthenticateBy(string username, string password);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator ConfigureConnectionPool(System.Action<Gremlin.Net.Driver.ConnectionPoolSettings> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator ConfigureGremlinClient(System.Func<Gremlin.Net.Driver.IGremlinClient, Gremlin.Net.Driver.IGremlinClient> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator SetAlias(string alias);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator SetSerializationFormat(ExRam.Gremlinq.Core.SerializationFormat version);
    }
    public static class WebSocketGremlinQueryEnvironmentBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator At(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator builder, string uri) { }
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator AtLocalhost(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator builder) { }
    }
    public static class WebSocketGremlinqOptions
    {
        public static ExRam.Gremlinq.Core.GremlinqOption<Newtonsoft.Json.Formatting> QueryLogFormatting;
        public static ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.GroovyFormatting> QueryLogGroovyFormatting;
        public static ExRam.Gremlinq.Core.GremlinqOption<Microsoft.Extensions.Logging.LogLevel> QueryLogLogLevel;
        public static ExRam.Gremlinq.Core.GremlinqOption<ExRam.Gremlinq.Core.QueryLogVerbosity> QueryLogVerbosity;
    }
}