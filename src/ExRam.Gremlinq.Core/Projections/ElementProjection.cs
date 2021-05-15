namespace ExRam.Gremlinq.Core.Projections
{
    public class ElementProjection : Projection
    {
        public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection BaseProjection => None;
    }
}
