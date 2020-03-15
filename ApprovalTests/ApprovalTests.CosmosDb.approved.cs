[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core
{
    public static class CosmosDbConfigurationBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilderWithUri At(this ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilder builder, string uri, string databaseName, string graphName) { }
    }
    public sealed class CosmosDbKey
    {
        public CosmosDbKey(string id) { }
        public CosmosDbKey(string partitionKey, string id) { }
        public string Id { get; }
        public string? PartitionKey { get; }
    }
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseCosmosDb(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment env, System.Func<ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilder, ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilderWithAuthKey> transformation) { }
    }
    public static class GremlinQuerySerializerExtensions { }
    public interface ICosmosDbConfigurationBuilder
    {
        ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilderWithUri At(System.Uri uri, string databaseName, string graphName);
    }
    public interface ICosmosDbConfigurationBuilderWithAuthKey : ExRam.Gremlinq.Providers.WebSocket.IGremlinQueryEnvironmentBuilder
    {
        ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilderWithAuthKey ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurationBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurationBuilder> transformation);
    }
    public interface ICosmosDbConfigurationBuilderWithUri
    {
        ExRam.Gremlinq.Core.ICosmosDbConfigurationBuilderWithAuthKey AuthenticateBy(string authKey);
    }
}