using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class MetaPropertyStep : Step
    {
        public MetaPropertyStep(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Key { get; }
        public object Value { get; }
    }
}
