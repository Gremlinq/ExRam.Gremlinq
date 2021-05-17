namespace ExRam.Gremlinq.Core.Projections
{
    public class EdgeOrVertexProjection : ElementProjection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection Lower() => Element;
    }
}
