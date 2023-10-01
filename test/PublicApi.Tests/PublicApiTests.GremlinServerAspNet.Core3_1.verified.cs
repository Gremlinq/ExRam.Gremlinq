namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Providers.Core.AspNet.IGremlinqProviderServicesBuilder<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup) { }
    }
}