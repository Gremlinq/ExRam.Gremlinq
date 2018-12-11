namespace ExRam.Gremlinq
{
    public sealed class InjectStep : Step
    {
        public object[] Elements { get; }

        public InjectStep(object[] elements)
        {
            Elements = elements;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}