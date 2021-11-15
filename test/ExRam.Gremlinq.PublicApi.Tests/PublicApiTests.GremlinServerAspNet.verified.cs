namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseGremlinServer(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator>>? configuration = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseGremlinServer<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator>>? configuration = null) { }
    }
}