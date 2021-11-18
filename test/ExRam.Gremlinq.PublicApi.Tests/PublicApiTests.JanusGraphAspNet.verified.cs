namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseJanusGraph(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>>? extraSetupAction = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseJanusGraph<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>>? extraSetupAction = null) { }
    }
}