using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertyStep : Step
    {
        public PropertyStep(Type type, object key, object value)
        {
            Key = key;
            Type = type;
            Value = value;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Type Type { get; }
        public object Key { get; }
        public object Value { get; }
    }
}
