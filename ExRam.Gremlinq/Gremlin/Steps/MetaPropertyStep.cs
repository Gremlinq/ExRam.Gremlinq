namespace ExRam.Gremlinq
{
    public sealed class MetaPropertyStep : Step
    {
        public MetaPropertyStep(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Key { get; }
        public object Value { get; }
    }
}