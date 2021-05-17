namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class EdgeOrVertexProjection : Projection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection Lower() => Element;
    }
}
