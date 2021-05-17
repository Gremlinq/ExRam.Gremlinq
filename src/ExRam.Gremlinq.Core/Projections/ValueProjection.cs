namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class ValueProjection : Projection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection Lower() => Empty;
    }
}
