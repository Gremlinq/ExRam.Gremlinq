namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup) { }
        public static ExRam.Gremlinq.Core.AspNet.GremlinqSetup UseNeptune<TVertex, TEdge>(this ExRam.Gremlinq.Core.AspNet.GremlinqSetup setup) { }
    }
}