namespace ExRam.Gremlinq.Core.Projections
{
    public class EdgeOrVertexProjection : ElementProjection
    {
        public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

        public override Projection BaseProjection => Element;
    }
}
