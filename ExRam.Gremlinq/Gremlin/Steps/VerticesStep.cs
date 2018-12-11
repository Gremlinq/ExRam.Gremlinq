namespace ExRam.Gremlinq
{
    public sealed class VerticesStep : SingleTraversalArgumentStep
    {
        public VerticesStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
