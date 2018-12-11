namespace ExRam.Gremlinq
{
    public sealed class UntilStep : SingleTraversalArgumentStep
    {
        public UntilStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
