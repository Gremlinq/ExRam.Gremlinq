namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseGremlinServer<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup, System.Func<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator, Microsoft.Extensions.Configuration.IConfigurationSection, ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator>? configuration = null) { }
    }
}