using ExRam.Gremlinq.Core.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertyStep : Step
    {
        public PropertyStep(string key, object value) : this(default, key, value)
        {
        }

        public PropertyStep(Option<Cardinality> cardinality, string key, object value)
        {
            Key = key;
            Value = value;
            Cardinality = cardinality;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Key { get; }
        public object Value { get; }
        public Option<Cardinality> Cardinality { get; }
    }
}
