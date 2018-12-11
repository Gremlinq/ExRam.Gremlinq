namespace ExRam.Gremlinq
{
    public sealed class OptionalStep : SingleTraversalArgumentStep
    {
        public OptionalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
