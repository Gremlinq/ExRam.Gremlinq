namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.AspNet.IProviderConfiguration, ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>? extraConfiguration = null) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, Microsoft.Extensions.Configuration.IConfiguration, ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>? extraConfiguration = null) { }
    }
}