using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

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
                sourceExpression = operand.Strip();
                return true;
            }

            sourceExpression = null;
            return false;
        }

        public static bool IsEquals(this Expression expression, [NotNullWhen(true)] out Expression? argument)
        {
            if (expression is MethodCallExpression { Method: { Name: nameof(object.Equals) } methodInfo, Arguments: [{ } argumentExpression] } && methodInfo.GetParameters().Length == 1 && methodInfo.ReturnType == typeof(bool))
                argument = argumentExpression;
            else
                argument = null;

            return argument is not null;
        }

        public static bool IsListContains(this Expression expression, [NotNullWhen(true)] out Expression? argument)
        {
            if (expression is MethodCallExpression { Method: { Name: nameof(List<object>.Contains) } methodInfo, Arguments: [{ } argumentExpression] } && typeof(IList).IsAssignableFrom(methodInfo.DeclaringType))
                argument = argumentExpression;
            else
                argument = null;

            return argument is not null;
        }

        private static bool IsMemberAndNamed<T>(this Expression expression, string name, [NotNullWhen(true)] out Expression? propertyExpression)
        {
            if (expression is MemberExpression { Expression: { } memberExpressionExpression, Member: { } member })
            {
                if (typeof(T).IsAssignableFrom(member.DeclaringType) && member.Name == name)
                {
                    propertyExpression = memberExpressionExpression.Strip();
                    return true;
                }
            }

            propertyExpression = null;
            return false;
        }
    }
}
