namespace ExRam.Gremlinq
{
    public sealed class NotStep : SingleTraversalArgumentStep
    {
        public NotStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
