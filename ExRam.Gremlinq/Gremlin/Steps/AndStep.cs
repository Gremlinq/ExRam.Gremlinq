namespace ExRam.Gremlinq
{
    public sealed class AndStep : LogicalStep
    {
        public AndStep(IGremlinQuery[] traversals) : base("and", traversals)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
