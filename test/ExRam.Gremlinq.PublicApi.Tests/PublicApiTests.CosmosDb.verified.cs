namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseCosmosDb(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator At(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator configurator, string uri, string databaseName, string graphName) { }
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator At(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator configurator, System.Uri uri, string databaseName, string graphName) { }
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator AtLocalhost(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator configurator, string databaseName, string graphName) { }
    }
    public readonly struct CosmosDbKey
    {
        public CosmosDbKey(string id) { }
        public CosmosDbKey(string partitionKey, string id) { }
        public string Id { get; }
        public string? PartitionKey { get; }
    }
    public interface ICosmosDbConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>, ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>
    {
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator AuthenticateBy(string authKey);
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator OnDatabase(string databaseName);
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator OnGraph(string graphName);
    }
}