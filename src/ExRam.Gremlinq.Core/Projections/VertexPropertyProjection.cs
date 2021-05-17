namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class VertexPropertyProjection : ElementProjection
    {
        public override Projection Lower() => Element;
    }
}
