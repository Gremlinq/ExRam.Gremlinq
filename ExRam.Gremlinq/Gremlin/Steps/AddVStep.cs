namespace ExRam.Gremlinq
{
    public sealed class AddVStep : AddElementStep
    {
        public AddVStep(object value) : base(value)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
