namespace ExRam.Gremlinq
{
    public sealed class EdgesStep : SingleTraversalArgumentStep
    {
        public EdgesStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
