using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal static class WellKnownMemberExtensions
    {
        public static bool IsPropertyValue(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<Property>(nameof(Property<>.Value), out sourceExpression);

        public static bool IsPropertyKey(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<Property>(nameof(Property<>.Key), out sourceExpression);

        public static bool IsStepLabelValue(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<StepLabel>(nameof(StepLabel<>.Value), out sourceExpression);

        public static bool IsVertexPropertyLabel(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<IVertexProperty>(nameof(VertexProperty<>.Label), out sourceExpression);

        public static bool IsVertexPropertyId(this Expression expression, [NotNullWhen(true)] out Expression? sourceExpression) => expression.IsMemberAndNamed<IVertexProperty>(nameof(VertexProperty<>.Id), out sourceExpression);

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

        public static bool LooksLikeCollectionContains(this Expression expression, [NotNullWhen(true)] out Expression? target, [NotNullWhen(true)] out Expression? argument)
        {
            target = null;
            argument = null;

            if (expression is MethodCallExpression { Method: { Name: nameof(Enumerable.Contains) } methodInfo, Arguments: [{ } firstArgumentExpression, ..] methodCallArguments} methodCallExpression && !typeof(string).IsAssignableFrom(methodInfo.DeclaringType))
            {
                var lastArgumentIsNullEqualityComparer = methodCallArguments[^1].Strip() is ConstantExpression { Value: null, Type: { IsGenericType: true } constantExpressionType } && constantExpressionType.GetGenericTypeDefinition() == typeof(IEqualityComparer<>);

                if (methodCallExpression.Object is { } instanceExpression)
                {
                    target = instanceExpression;

                    if (methodCallArguments is [_] || (methodCallArguments is [_, _] && lastArgumentIsNullEqualityComparer))
                        argument = firstArgumentExpression;
                }
                else
                {
                    target = firstArgumentExpression;

                    if (methodCallArguments is [_, { } secondArgumentExpression, ..])
                    {
                        if (methodCallArguments is [_, _] || (methodCallArguments is [_, _, _] && lastArgumentIsNullEqualityComparer))
                            argument = secondArgumentExpression;
                    }
                }
            }

            return argument is not null;
        }

        public static bool IsIndexerGet(this Expression expression, [NotNullWhen(true)] out Expression? target, [NotNullWhen(true)] out Expression? argument)
        {
            if (expression is MethodCallExpression { Object: { } targetExpression, Method: { Name: "get_Item", DeclaringType: { IsGenericType: true } declaringType }, Arguments: [{ } argumentExpression] } && declaringType.GetGenericArguments() is [_, _])
            {
                target = targetExpression;
                argument = argumentExpression;
            }
            else
            {
                target = null;
                argument = null;
            }

            return argument is not null;
        }

        public static bool IsCompareTo(this Expression expression, [NotNullWhen(true)] out Expression? target, [NotNullWhen(true)] out Expression? comparand, out int compareToValue)
        {
            if (expression is BinaryExpression { Left: MethodCallExpression { Object: { } targetExpression, Method: { Name: nameof(IComparable.CompareTo) } methodInfo, Arguments: [{ } firstArgument, ..] } } binaryExpression && methodInfo.GetParameters().Length == 1 && methodInfo.ReturnType == typeof(int) && binaryExpression.Right.GetValue() is IConvertible convertible)
            {
                target = targetExpression;
                comparand = firstArgument;

                try
                {
                    compareToValue = convertible.ToInt32(CultureInfo.InvariantCulture);

                    return true;
                }
                catch(FormatException)
                {

                }
            }

            target = null;
            comparand = null;
            compareToValue = 0;

            return false;
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
