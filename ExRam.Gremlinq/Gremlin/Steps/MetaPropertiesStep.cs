namespace ExRam.Gremlinq
{
    public sealed class MetaPropertiesStep : Step
    {
        public MetaPropertiesStep(string[] keys)
        {
            Keys = keys;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string[] Keys { get; }
    }
}
