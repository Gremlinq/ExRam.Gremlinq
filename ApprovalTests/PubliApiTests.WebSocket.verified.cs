[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment ConfigureWebSocket(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder, ExRam.Gremlinq.Core.IGremlinQueryEnvironmentBuilder> builderAction) { }
    }
}
namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketGremlinQueryEnvironmentBuilder : ExRam.Gremlinq.Core.IGremlinQueryEnvironmentBuilder
    {
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder AddGraphSONDeserializer(string typename, Gremlin.Net.Structure.IO.GraphSON.IGraphSONDeserializer serializer);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder AddGraphSONSerializer(System.Type type, Gremlin.Net.Structure.IO.GraphSON.IGraphSONSerializer serializer);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder At(System.Uri uri);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder AuthenticateBy(string username, string password);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder ConfigureConnectionPool(System.Action<Gremlin.Net.Driver.ConnectionPoolSettings> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder ConfigureGremlinClient(System.Func<Gremlin.Net.Driver.IGremlinClient, Gremlin.Net.Driver.IGremlinClient> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder ConfigureQueryLoggingOptions(System.Func<ExRam.Gremlinq.Providers.WebSocket.QueryLoggingOptions, ExRam.Gremlinq.Providers.WebSocket.QueryLoggingOptions> transformation);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder SetAlias(string alias);
        ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder SetSerializationFormat(ExRam.Gremlinq.Core.SerializationFormat version);
    }
    public readonly struct QueryLoggingOptions
    {
        public static readonly ExRam.Gremlinq.Providers.WebSocket.QueryLoggingOptions Default;
        public QueryLoggingOptions(Microsoft.Extensions.Logging.LogLevel logLevel, ExRam.Gremlinq.Providers.WebSocket.QueryLoggingVerbosity verbosity, Newtonsoft.Json.Formatting formatting) { }
        public Newtonsoft.Json.Formatting Formatting { get; }
        public Microsoft.Extensions.Logging.LogLevel LogLevel { get; }
        public ExRam.Gremlinq.Providers.WebSocket.QueryLoggingVerbosity Verbosity { get; }
        public ExRam.Gremlinq.Providers.WebSocket.QueryLoggingOptions SetFormatting(Newtonsoft.Json.Formatting formatting) { }
        public ExRam.Gremlinq.Providers.WebSocket.QueryLoggingOptions SetLogLevel(Microsoft.Extensions.Logging.LogLevel logLevel) { }
        public ExRam.Gremlinq.Providers.WebSocket.QueryLoggingOptions SetQueryLoggingVerbosity(ExRam.Gremlinq.Providers.WebSocket.QueryLoggingVerbosity verbosity) { }
    }
    public enum QueryLoggingVerbosity
    {
        None = 0,
        QueryOnly = 1,
        QueryAndParameters = 2,
    }
    public static class WebSocketGremlinQueryEnvironmentBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder At(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder builder, string uri) { }
        public static ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder AtLocalhost(this ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder builder) { }
    }
}