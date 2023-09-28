namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseCosmosDb<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase>, ExRam.Gremlinq.Providers.Core.AspNet.IProviderConfigurationSection, ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase>>? configuratorTransformation = null) { }
    }
}