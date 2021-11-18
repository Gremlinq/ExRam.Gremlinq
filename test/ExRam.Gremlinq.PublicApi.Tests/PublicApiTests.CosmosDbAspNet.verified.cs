namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseCosmosDb(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>>? extraSetupAction = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseCosmosDb<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Linq.Expressions.Expression<System.Func<TVertex, object>> partitionKeyExpression, System.Action<ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>>? extraSetupAction = null) { }
    }
}