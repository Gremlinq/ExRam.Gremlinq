using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class PropertyStep : Step
    {
        public PropertyStep(IGraphModel model, PropertyInfo property, object value) : this(property.PropertyType, model.GetIdentifier(property.Name), value)
        {
        }

        public PropertyStep(IGraphModel model, MemberExpression memberExpression, object value) : this(memberExpression.Type, model.GetIdentifier(memberExpression.Member.Name), value)
        {
        }

        private PropertyStep(Type type, object key, object value)
        {
            Key = key;
            Type = type;
            Value = value;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Type Type { get; }
        public object Key { get; }
        public object Value { get; }
    }
}
