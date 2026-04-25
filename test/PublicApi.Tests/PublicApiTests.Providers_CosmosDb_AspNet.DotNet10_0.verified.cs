namespace ExRam.Gremlinq.Providers.CosmosDb.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        [System.Obsolete(@"This method and the CosmosDb provider packages will be removed in ExRam.Gremlinq v14.

To keep using the CosmosDb provider packages beyond ExRam.Gremlinq v13, subscribe to any of the Gremlinq.Extensions offers (https://docs.gremlinq.net/extensions/).

Existing customers of any Gremlinq.Extensions product already have access to all ExRam.Gremlinq.Providers.CosmosDb.* packages on the Gremlinq.Extensions NuGet feed.
Simply update to the latest 13.x version, and this message will disappear.

For details on the v14 transition and available options, see https://docs.gremlinq.net/cosmosdb-provider-packages/", false)]
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<ExRam.Gremlinq.Providers.CosmosDb.ICosmosDbConfigurator<TVertexBase>> UseCosmosDb<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup) { }
    }
}