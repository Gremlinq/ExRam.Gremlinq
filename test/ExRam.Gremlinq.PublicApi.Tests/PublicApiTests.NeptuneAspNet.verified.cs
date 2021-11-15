namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>>? extraSetupAction = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Action<ExRam.Gremlinq.Core.AspNet.ProviderSetup<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>>? extraSetupAction = null) { }
    }
}