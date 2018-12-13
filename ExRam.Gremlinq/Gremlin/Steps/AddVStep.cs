namespace ExRam.Gremlinq
{
    public sealed class AddVStep : AddElementStep
    {
        public AddVStep(IGraphModel model, object value) : base(model, value)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
