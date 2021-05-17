namespace ExRam.Gremlinq.Core.Projections
{
    public class ElementProjection : Projection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection Lower() => Empty;
    }
}
