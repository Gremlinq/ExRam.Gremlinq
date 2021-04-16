namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseCosmosDb(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> transformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class CosmosDbConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator At(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator configurator, string uri, string databaseName, string graphName) { }
    }
    public readonly struct CosmosDbKey
    {
        public CosmosDbKey(string id) { }
        public CosmosDbKey(string partitionKey, string id) { }
        public string Id { get; }
        public string? PartitionKey { get; }
    }
    public interface ICosmosDbConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.WebSocket.IProviderConfigurator<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>
    {
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator At(System.Uri uri, string databaseName, string graphName);
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator AuthenticateBy(string authKey);
    }
}