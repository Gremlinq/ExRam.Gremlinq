using System;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
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
