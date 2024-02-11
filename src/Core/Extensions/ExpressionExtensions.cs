using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Core.ExpressionParsing;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionExtensions
    {
        public static Expression StripConvert(this Expression expression)
        {
            while (true)
            {
                switch (expression)
                {
                    case MemberExpression { Member: PropertyInfo { Name: "Value" }, Expression: { } memberExpressionExpression } when Nullable.GetUnderlyingType(memberExpressionExpression.Type) is not null:
                    {
                        expression = memberExpressionExpression;
                        break;
                    }
                    case UnaryExpression unaryExpression when expression.NodeType == ExpressionType.Convert:
                    {
                        expression = unaryExpression.Operand;
                        break;
                    }
                    case MethodCallExpression { Object: { } objectExpression } methodCallExpression when methodCallExpression.Method == WellKnownMethods.ObjectToString:
                    {
                        expression = objectExpression;
                        break;
                    }
                    default:
                    {
                        return expression;
                    }
                }
            }
        }

        public static MemberExpression AssumeMemberExpression(this Expression expression)
        {
            return expression.StripConvert() switch
            {
                LambdaExpression lambdaExpression => lambdaExpression.Body.AssumeMemberExpression(),
                MemberExpression memberExpression => memberExpression,
                _ => throw new ExpressionNotSupportedException(expression)
            };
        }

        public static MemberExpression AssumePropertyOrFieldMemberExpression(this Expression expression)
        {
            return expression.AssumeMemberExpression() is { Member: { } member } memberExpression && (member is FieldInfo || member is PropertyInfo)
                ? memberExpression
                : throw new ExpressionNotSupportedException(expression);
        }

        public static object? GetValue(this Expression expression)
        {
            return expression switch
            {
                ConstantExpression constantExpression => constantExpression.Value,
                MethodCallExpression methodCallExpression => methodCallExpression.Method.Invoke(
                    methodCallExpression.Object?.GetValue(),
                    methodCallExpression.GetArguments()),
                MemberExpression memberExpression when memberExpression.IsStepLabelValue(out var stepLabelExpression) => stepLabelExpression.GetValue(),
                MemberExpression { Expression: { } sourceExpression } when sourceExpression.IsStepLabelValue(out var stepLabelExpression) => stepLabelExpression.GetValue(),
                MemberExpression { Member: PropertyInfo propertyInfo } propertyExpression => propertyInfo.GetValue(propertyExpression.Expression?.GetValue()),
                MemberExpression { Member: FieldInfo fieldInfo } fieldExpression => fieldInfo.GetValue(fieldExpression.Expression?.GetValue()),
                NewExpression { Constructor: { } constructor, Members: null } newExpression => constructor.Invoke(newExpression.GetArguments()),
                NewArrayExpression newArrayExpression => newArrayExpression.GetValue(),
                _ => Expression.Lambda<Func<object>>(expression.Type.IsClass ? expression : Expression.Convert(expression, typeof(object))).Compile()()
            };
        }

        public static WellKnownOperation? TryGetWellKnownOperation(this MethodCallExpression expression)
        {
            var methodInfo = expression.Method;

            if (methodInfo.IsStatic)
            {
                var thisExpression = expression.Arguments[0].StripConvert();

                if (methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == WellKnownMethods.EnumerableAny)
                {
                    return thisExpression is MethodCallExpression { Method.IsGenericMethod: true } previousMethodCallExpression && previousMethodCallExpression.Method.GetGenericMethodDefinition() == WellKnownMethods.EnumerableIntersect
                        ? WellKnownOperation.EnumerableIntersectAny
                        : WellKnownOperation.EnumerableAny;
                }

                if (methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == WellKnownMethods.EnumerableContainsElement)
                    return WellKnownOperation.EnumerableContains;
            }
            else
            {
                if (typeof(IList).IsAssignableFrom(methodInfo.DeclaringType) && methodInfo.Name == nameof(List<object>.Contains))
                    return WellKnownOperation.ListContains;

                if (methodInfo.DeclaringType is { IsGenericType: true } declaringType && declaringType.GetGenericArguments() is [_, _] && methodInfo.Name == "get_Item")
                    return WellKnownOperation.IndexerGet;

                if (methodInfo.DeclaringType == typeof(string) && methodInfo.GetParameters() is { Length: 1 or 2 } parameters)
                {
                    if (parameters[0].ParameterType == typeof(string) && (parameters.Length == 1 || parameters[1].ParameterType == typeof(StringComparison)))
                    {
                        switch (methodInfo.Name)
                        {
                            case nameof(object.Equals):
                                return WellKnownOperation.StringEquals;
                            case nameof(string.StartsWith):
                                return WellKnownOperation.StringStartsWith;
                            case nameof(string.EndsWith):
                                return WellKnownOperation.StringEndsWith;
                            case nameof(string.Contains):
                                return WellKnownOperation.StringContains;
                        }
                    }
                }

                if (methodInfo.Name == nameof(object.Equals) && methodInfo.GetParameters().Length == 1 && methodInfo.ReturnType == typeof(bool))
                    return WellKnownOperation.Equals;

                if (methodInfo.Name == nameof(IComparable.CompareTo) && methodInfo.GetParameters().Length == 1 && methodInfo.ReturnType == typeof(int))
                    return WellKnownOperation.ComparableCompareTo;
            }

            return null;
        }

        public static bool RefersToStepLabel(this Expression expression, [NotNullWhen(true)] out StepLabel? stepLabel, out MemberExpression? stepLabelValueMemberExpression)
        {
            stepLabel = null;
            stepLabelValueMemberExpression = null;

            if (typeof(StepLabel).IsAssignableFrom(expression.Type))
                stepLabel = (StepLabel?)expression.GetValue();
            else if (expression is MemberExpression outerMemberExpression)
            {
                if (outerMemberExpression.IsStepLabelValue(out var stepLabelExpression))
                    stepLabel = (StepLabel?)stepLabelExpression?.GetValue();
                else if (outerMemberExpression.Expression is MemberExpression innerMemberExpression && innerMemberExpression.IsStepLabelValue(out stepLabelExpression))
                {
                    stepLabelValueMemberExpression = outerMemberExpression;
                    stepLabel = (StepLabel?)stepLabelExpression?.GetValue();
                }
            }

            return stepLabel is not null;
        }

        public static bool RefersToParameter(this Expression expression, [NotNullWhen(true)] out ParameterExpression? parameter)
        {
            var actualExpression = (Expression?)expression;

            while (true)
            {
                if (actualExpression is null)
                    break;

                actualExpression = actualExpression.StripConvert();

                switch (actualExpression)
                {
                    case ParameterExpression parameterExpression:
                    {
                        parameter = parameterExpression;
                        return true;
                    }
                    case LambdaExpression lambdaExpression:
                    {
                        actualExpression = lambdaExpression.Body;
                        break;
                    }
                    case MemberExpression memberExpression:
                    {
                        actualExpression = memberExpression.Expression;
                        break;
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        actualExpression = methodCallExpression.Object;
                        break;
                    }
                    case UnaryExpression unaryExpression:
                    {
                        actualExpression = unaryExpression.Operand;
                        break;
                    }
                    default:
                    {
                        actualExpression = null;
                        break;
                    }
                }
            }

            parameter = null;
            return false;
        }

        public static bool IsIdentityExpression(this LambdaExpression expression)
        {
            return expression.Parameters.Count == 1 && expression.Body.StripConvert() == expression.Parameters[0];
        }

        public static WhereExpression? TryParseWhereExpression(this Expression body)
        {
            switch (body)
            {
                case MemberExpression { Member: PropertyInfo property } memberExpression when property.PropertyType == typeof(bool) && memberExpression.RefersToParameter(out _):
                {
                    return new WhereExpression(
                        memberExpression,
                        EqualsExpressionSemantics.Instance,
                        Expressions.True);
                }
                case BinaryExpression binaryExpression when binaryExpression.NodeType.TryToSemantics(out var semantics):
                {
                    if (binaryExpression.Left is MethodCallExpression { Object: { } target, Arguments: [ { } firstArgument, ..] } leftMethodCallExpression && semantics is ObjectExpressionSemantics objectExpressionSemantics && leftMethodCallExpression.TryGetWellKnownOperation() == WellKnownOperation.ComparableCompareTo && binaryExpression.Right.GetValue() is IConvertible convertible)
                    {
                        try
                        {
                            var transformedSemantics = objectExpressionSemantics.TransformCompareTo(convertible.ToInt32(CultureInfo.InvariantCulture));

                            return transformedSemantics switch
                            {
                                TrueExpressionSemantics => WhereExpression.True,
                                FalseExpressionSemantics => WhereExpression.False,
                                _ => new WhereExpression(
                                    target,
                                    transformedSemantics,
                                    firstArgument)
                            };
                        }
                        catch (FormatException)
                        {

                        }
                    }
                    else
                    { 
                        return new WhereExpression(
                            binaryExpression.Left,
                            semantics,
                            binaryExpression.Right);
                    }

                    break;
                }
                case MethodCallExpression { Object: { } targetExpression, Arguments: [var firstArgument, ..] } instanceMethodCallExpression:
                {
                    var wellKnownMember = instanceMethodCallExpression.TryGetWellKnownOperation();

                    switch (wellKnownMember)
                    {
                        case WellKnownOperation.Equals:
                        {
                            return new WhereExpression(
                                targetExpression,
                                EqualsExpressionSemantics.Instance,
                                firstArgument);
                        }
                        case WellKnownOperation.ListContains:
                        {
                            return new WhereExpression(
                                targetExpression,
                                ContainsExpressionSemantics.Instance,
                                firstArgument);
                        }
                        case WellKnownOperation.StringEquals:
                        case WellKnownOperation.StringStartsWith:
                        case WellKnownOperation.StringEndsWith:
                        case WellKnownOperation.StringContains:
                        {
                            var instanceExpression = targetExpression.StripConvert();
                            var argumentExpression = firstArgument.StripConvert();

                            var stringComparison = instanceMethodCallExpression.Arguments is [_, { } secondArgument, ..] && secondArgument.Type == typeof(StringComparison)
                                ? (StringComparison)secondArgument.GetValue()!
                                : StringComparison.Ordinal;

                            if (wellKnownMember == WellKnownOperation.StringStartsWith && argumentExpression.RefersToParameter(out _))
                            {
                                if (instanceExpression.GetValue()?.ToString() is { } stringValue)
                                {
                                    return new WhereExpression(
                                        Expression.Constant(stringValue),
                                        StartsWithExpressionSemantics.Get(stringComparison),
                                        argumentExpression);
                                }
                            }
                            else if (instanceExpression.RefersToParameter(out _))
                            {
                                if (argumentExpression.GetValue() is string stringValue)
                                {
                                    return new WhereExpression(
                                        instanceExpression,
                                        wellKnownMember switch
                                        {
                                            WellKnownOperation.StringEquals => StringEqualsExpressionSemantics.Get(stringComparison),
                                            WellKnownOperation.StringStartsWith => StartsWithExpressionSemantics.Get(stringComparison),
                                            WellKnownOperation.StringContains => HasInfixExpressionSemantics.Get(stringComparison),
                                            WellKnownOperation.StringEndsWith => EndsWithExpressionSemantics.Get(stringComparison),
                                            _ => throw new ExpressionNotSupportedException(instanceMethodCallExpression)
                                        },
                                        Expression.Constant(stringValue));
                                }
                            }

                            break;
                        }
                    }

                    break;
                }
                case MethodCallExpression { Object: null, Arguments: [var firstArgument, ..] } staticMethodCallExpression:
                {
                    var wellKnownMember = staticMethodCallExpression.TryGetWellKnownOperation();

                    switch (wellKnownMember)
                    {
                        case WellKnownOperation.EnumerableIntersectAny when firstArgument.StripConvert() is MethodCallExpression { Arguments: [var anyTarget, var anyArgument] }:
                        {
                            return new WhereExpression(
                                anyTarget,
                                IntersectsExpressionSemantics.Instance,
                                anyArgument);
                        }
                        case WellKnownOperation.EnumerableAny:
                        {
                            return new WhereExpression(
                                firstArgument,
                                NotEqualsExpressionSemantics.Instance,
                                Expressions.Null);
                        }
                        case WellKnownOperation.EnumerableContains when staticMethodCallExpression.Arguments is [_, var secondArgument]:
                        {
                            return new WhereExpression(
                                firstArgument,
                                ContainsExpressionSemantics.Instance,
                                secondArgument);
                        }
                    }

                    break;
                }
            }

            return default;
        }

        private static Array GetValue(this NewArrayExpression expression)
        {
            var array = Array.CreateInstance(
                expression.Type.GetElementType()!,
                expression.Expressions.Count);

            for (var i = 0; i < expression.Expressions.Count; i++)
            {
                array.SetValue(
                    expression.Expressions[i].GetValue(),
                    i);
            }

            return array;
        }

        private static object?[] GetArguments(this IArgumentProvider argumentProvider)
        {
            if (argumentProvider.ArgumentCount > 0)
            {
                var arguments = new object?[argumentProvider.ArgumentCount];

                for (var i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = argumentProvider.GetArgument(i).GetValue();
                }

                return arguments;
            }

            return Array.Empty<object>();
        }
    }
}
