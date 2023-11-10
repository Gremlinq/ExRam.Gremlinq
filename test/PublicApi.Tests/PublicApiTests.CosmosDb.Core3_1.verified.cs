namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseCosmosDb<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.IGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase>, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
    public static class CosmosDbConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> At<TVertexBase>(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> configurator, string uri, string databaseName, string graphName) { }
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> At<TVertexBase>(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> configurator, System.Uri uri, string databaseName, string graphName) { }
        public static ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> AtLocalhost<TVertexBase>(this ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> configurator, string databaseName, string graphName) { }
    }
    public readonly struct CosmosDbKey
    {
        public CosmosDbKey(string id) { }
        public CosmosDbKey(string partitionKey, string id) { }
        public string Id { get; }
        public string? PartitionKey { get; }
    }
    public interface ICosmosDbConfigurator<TVertexBase> : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase>>, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase>, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>>
    {
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> AuthenticateBy(string authKey);
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> OnDatabase(string databaseName);
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> OnGraph(string graphName);
        ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase> WithPartitionKey(System.Linq.Expressions.Expression<System.Func<TVertexBase, object>> partitionKeyExpression);
    }
}