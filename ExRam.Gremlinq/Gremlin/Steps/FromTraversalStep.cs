namespace ExRam.Gremlinq
{
    public sealed class FromTraversalStep : SingleTraversalArgumentStep
    {
        public FromTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
