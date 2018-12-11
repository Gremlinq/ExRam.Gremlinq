namespace ExRam.Gremlinq
{
    public sealed class AddEStep : AddElementStep
    {
        public AddEStep(object value) : base(value)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
