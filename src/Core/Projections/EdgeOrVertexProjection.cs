namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class EdgeOrVertexProjection : Projection
    {
        public override Projection Lower() => Element;
    }
}
