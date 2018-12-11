namespace ExRam.Gremlinq
{
    public sealed class ToTraversalStep : SingleTraversalArgumentStep
    {
        public ToTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
