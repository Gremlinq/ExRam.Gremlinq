namespace ExRam.Gremlinq
{
    public sealed class SideEffectStep : SingleTraversalArgumentStep
    {
        public SideEffectStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
