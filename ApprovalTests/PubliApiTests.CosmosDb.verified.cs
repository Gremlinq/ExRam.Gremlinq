[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseCosmosDb(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment env, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilder, ExRam.Gremlinq.Core.IGremlinQueryEnvironmentBuilder> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfigurationBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilderWithUri At(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilder builder, string uri, string databaseName, string graphName) { }
    }
    public readonly struct CosmosDbKey
    {
        public CosmosDbKey(string id) { }
        public CosmosDbKey(string partitionKey, string id) { }
        public string Id { get; }
        public string? PartitionKey { get; }
    }
    public interface ICosmosDbConfigurationBuilder
    {
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilderWithUri At(System.Uri uri, string databaseName, string graphName);
    }
    public interface ICosmosDbConfigurationBuilderWithAuthKey : ExRam.Gremlinq.Core.IGremlinQueryEnvironmentBuilder
    {
        ExRam.Gremlinq.Core.IGremlinQueryEnvironmentBuilder ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder> transformation);
    }
    public interface ICosmosDbConfigurationBuilderWithUri
    {
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilderWithAuthKey AuthenticateBy(string authKey);
    }
}