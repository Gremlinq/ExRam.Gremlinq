using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class PropertyStep : Step
    {
        public object Value { get; }
        public PropertyInfo Property { get; }
        public MemberExpression MemberExpression { get; }

        public PropertyStep(PropertyInfo property, object value)
        {
            Value = value;
            Property = property;
        }

        public PropertyStep(MemberExpression memberExpression, object value)
        {
            Value = value;
            MemberExpression = memberExpression;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
