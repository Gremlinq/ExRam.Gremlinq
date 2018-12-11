namespace ExRam.Gremlinq
{
    public sealed class RepeatStep : SingleTraversalArgumentStep
    {
        public RepeatStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
