namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseCosmosDb(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator, ExRam.Gremlinq.Core.AspNet.IProviderConfiguration, ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>? extraConfiguration = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseCosmosDb<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Linq.Expressions.Expression<System.Func<TVertex, object>> partitionKeyExpression, System.Func<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator, Microsoft.Extensions.Configuration.IConfiguration, ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator>? extraConfiguration = null) { }
    }
}