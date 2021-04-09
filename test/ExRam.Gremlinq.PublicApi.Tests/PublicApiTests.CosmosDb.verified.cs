namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseCosmosDb(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilder, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
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
    public interface ICosmosDbConfigurationBuilderWithAuthKey : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation
    {
        ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator> transformation);
    }
    public interface ICosmosDbConfigurationBuilderWithUri
    {
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurationBuilderWithAuthKey AuthenticateBy(string authKey);
    }
}