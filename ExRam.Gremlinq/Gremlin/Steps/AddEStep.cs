namespace ExRam.Gremlinq
{
    public sealed class AddEStep : AddElementStep
    {
        public AddEStep(IGraphModel model, object value) : base(model, value)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
