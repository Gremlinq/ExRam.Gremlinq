namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, Microsoft.Extensions.Configuration.IConfigurationSection, ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>? configuration = null) { }
    }
}