namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class EmptyProjection : Projection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection BaseProjection => Empty;
    }
}
