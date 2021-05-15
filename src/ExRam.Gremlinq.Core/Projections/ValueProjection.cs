namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class ValueProjection : Projection
    {
        public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection BaseProjection => None;
    }
}
