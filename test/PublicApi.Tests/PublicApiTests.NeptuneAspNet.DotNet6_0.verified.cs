namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Providers.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>>? extraSetupAction = null) { }
    }
}