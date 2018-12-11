namespace ExRam.Gremlinq
{
    public sealed class LocalStep : SingleTraversalArgumentStep
    {
        public LocalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
