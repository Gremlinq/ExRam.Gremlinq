namespace ExRam.Gremlinq
{
    public sealed class VerticesStep : SingleTraversalArgumentStep
    {
        public VerticesStep(IGremlinQuery traversal) : base("vertices", traversal)
        {
        }
    }
}