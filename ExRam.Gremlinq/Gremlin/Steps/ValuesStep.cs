namespace ExRam.Gremlinq
{
    public sealed class ValuesStep : Step
    {
        public object[] Keys { get; }

        public ValuesStep(object[] keys)
        {
            Keys = keys;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
