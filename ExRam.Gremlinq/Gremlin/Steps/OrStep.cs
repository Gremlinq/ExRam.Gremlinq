namespace ExRam.Gremlinq
{
    public sealed class OrStep : LogicalStep<OrStep>
    {
        public OrStep(IGremlinQuery[] traversals) : base("or", traversals)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
