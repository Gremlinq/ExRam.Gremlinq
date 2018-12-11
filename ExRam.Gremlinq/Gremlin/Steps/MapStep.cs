namespace ExRam.Gremlinq
{
    public sealed class MapStep : SingleTraversalArgumentStep
    {
        public MapStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
