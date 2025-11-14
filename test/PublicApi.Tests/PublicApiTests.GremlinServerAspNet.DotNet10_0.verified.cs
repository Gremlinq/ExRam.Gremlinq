namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator> UseGremlinServer<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup) { }
    }
}