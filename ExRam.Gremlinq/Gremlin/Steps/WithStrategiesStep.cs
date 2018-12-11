namespace ExRam.Gremlinq
{
    public sealed class WithStrategiesStep : SingleTraversalArgumentStep
    {
        public WithStrategiesStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
