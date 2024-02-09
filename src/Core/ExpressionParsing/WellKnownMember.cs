using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal static class WellKnownMemberExtensions
    {
        public static bool IsPropertyValue(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<Property>(nameof(Property<object>.Value), out sourceExpression);

        public static bool IsPropertyKey(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<Property>(nameof(Property<object>.Key), out sourceExpression);

        public static bool IsStepLabelValue(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<StepLabel>(nameof(StepLabel<object>.Value), out sourceExpression);

        public static bool IsVertexPropertyLabel(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<IVertexProperty>(nameof(VertexProperty<object>.Label), out sourceExpression);

        public static bool IsArrayLength(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression)
        {
            if (expression is UnaryExpression { NodeType: ExpressionType.ArrayLength, Operand: { } operand })
            {
                sourceExpression = operand.StripConvert();
                return true;
            }

            sourceExpression = null;
            return false;
        }

        private static bool IsMemberAndNamed<T>(this Expression expression, string name, [NotNullWhen(true)] out Expression? propertyExpression)
        {
            if (expression is MemberExpression { Expression: { } memberExpressionExpression, Member: { } member })
            {
                if (typeof(T).IsAssignableFrom(member.DeclaringType) && member.Name == name)
                {
                    propertyExpression = memberExpressionExpression.StripConvert();
                    return true;
                }
            }

            propertyExpression = null;
            return false;
        }
    }

    internal enum WellKnownMember
    {
        PropertyValue,
        PropertyKey,
        StepLabelValue,
        VertexPropertyLabel,

        ArrayLength,
    }
}
