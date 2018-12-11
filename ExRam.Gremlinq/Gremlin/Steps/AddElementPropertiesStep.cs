namespace ExRam.Gremlinq
{
    public sealed class AddElementPropertiesStep : Step
    {
        public AddElementPropertiesStep(object element)
        {
            Element = element;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public object Element { get; }
    }
}
