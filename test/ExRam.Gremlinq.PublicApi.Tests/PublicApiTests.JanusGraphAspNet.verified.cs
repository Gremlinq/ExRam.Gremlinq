namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseJanusGraph(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, ExRam.Gremlinq.Core.AspNet.IProviderConfiguration, ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>? extraConfiguration = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseJanusGraph<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator, Microsoft.Extensions.Configuration.IConfiguration, ExRam.Gremlinq.Providers.JanusGraph.IJanusGraphConfigurator>? extraConfiguration = null) { }
    }
}