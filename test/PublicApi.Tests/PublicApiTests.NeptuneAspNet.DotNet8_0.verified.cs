namespace ExRam.Gremlinq.Providers.Neptune.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator> UseNeptune<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder setup) { }
    }
}