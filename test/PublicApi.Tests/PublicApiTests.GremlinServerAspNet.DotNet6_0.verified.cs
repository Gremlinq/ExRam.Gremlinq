namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderSetup<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.IGremlinqSetup setup) { }
    }
}