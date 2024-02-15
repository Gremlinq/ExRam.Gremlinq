using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.ExpressionParsing;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionExtensions
    {
        public static Expression Strip(this Expression expression)
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

        public static MemberExpression AssumeMemberExpression(this Expression expression) => expression.Strip() switch
        {
            LambdaExpression lambdaExpression => lambdaExpression.Body.AssumeMemberExpression(),
            MemberExpression memberExpression => memberExpression,
            _ => throw new ExpressionNotSupportedException(expression)
        };

        public static MemberExpression AssumePropertyOrFieldMemberExpression(this Expression expression) => expression.AssumeMemberExpression() is { Member: { } member } memberExpression && (member is FieldInfo || member is PropertyInfo)
            ? memberExpression
            : throw new ExpressionNotSupportedException(expression);

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

            while (actualExpression != null)
            {
                actualExpression = actualExpression.Strip();

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

        public static bool IsIdentityExpression(this LambdaExpression expression) => expression.Parameters.Count == 1 && expression.Body.Strip() == expression.Parameters[0];

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
                case BinaryExpression binaryExpression when binaryExpression.NodeType.TryToSemantics(out var semantics) && semantics is ObjectExpressionSemantics objectExpressionSemantics:
                {
                    if (binaryExpression.IsCompareTo(out var target, out var comparand, out var compareToValue))
                    {
                        var transformedSemantics = objectExpressionSemantics.TransformCompareTo(compareToValue);

                        return transformedSemantics switch
                        {
                            TrueExpressionSemantics => WhereExpression.True,
                            FalseExpressionSemantics => WhereExpression.False,
                            _ => new WhereExpression(
                                target,
                                transformedSemantics,
                                comparand)
                        };
                    }
                    else
                    {
                        return new WhereExpression(
                            binaryExpression.Left,
                            semantics,
                            binaryExpression.Right);
                    }
                }
                case MethodCallExpression { Object: { } targetExpression, Method: { } methodInfo, Arguments: [var firstArgument, ..] } instanceMethodCallExpression:
                {
                    if (instanceMethodCallExpression.IsEquals(out var equalsArgument))
                    {
                        return new WhereExpression(
                            targetExpression,
                            EqualsExpressionSemantics.Instance,
                            equalsArgument);
                    }

                    if (instanceMethodCallExpression.IsListContains(out var listContainsArgument))
                    {
                        return new WhereExpression(
                            targetExpression,
                            ContainsExpressionSemantics.Instance,
                            firstArgument);
                    }

                    if (methodInfo.DeclaringType == typeof(string) && methodInfo.GetParameters() is { Length: 1 or 2 } parameters)
                    {
                        if (parameters[0].ParameterType == typeof(string) && (parameters.Length == 1 || parameters[1].ParameterType == typeof(StringComparison)))
                        {
                            var instanceExpression = targetExpression.Strip();
                            var argumentExpression = firstArgument.Strip();

                            var stringComparison = instanceMethodCallExpression.Arguments is [_, { } secondArgument, ..] && secondArgument.Type == typeof(StringComparison)
                                ? (StringComparison)secondArgument.GetValue()!
                                : StringComparison.Ordinal;

                            if (methodInfo.Name == nameof(string.StartsWith) && argumentExpression.RefersToParameter(out _))
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
                                        methodInfo.Name switch
                                        {
                                            nameof(object.Equals) => StringEqualsExpressionSemantics.Get(stringComparison),
                                            nameof(string.StartsWith) => StartsWithExpressionSemantics.Get(stringComparison),
                                            nameof(string.Contains) => HasInfixExpressionSemantics.Get(stringComparison),
                                            nameof(string.EndsWith) => EndsWithExpressionSemantics.Get(stringComparison),
                                            _ => throw new ExpressionNotSupportedException(instanceMethodCallExpression)
                                        },
                                        Expression.Constant(stringValue));
                                }
                            }
                        }
                    }

                    break;
                }
                case MethodCallExpression { Object: null, Method: { } methodInfo, Arguments: [var firstArgument, ..] } staticMethodCallExpression:
                {
                    var thisExpression = firstArgument.Strip();

                    if (methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == WellKnownMethods.EnumerableAny)
                    {
                        if (thisExpression is MethodCallExpression { Method.IsGenericMethod: true } previousMethodCallExpression && previousMethodCallExpression.Method.GetGenericMethodDefinition() == WellKnownMethods.EnumerableIntersect)
                        {
                            if (firstArgument.Strip() is MethodCallExpression { Arguments: [var anyTarget, var anyArgument] })
                            {
                                return new WhereExpression(
                                    anyTarget,
                                    IntersectsExpressionSemantics.Instance,
                                    anyArgument);
                            }
                        }
                        else
                        {
                            return new WhereExpression(
                                firstArgument,
                                NotEqualsExpressionSemantics.Instance,
                                Expressions.Null);

                        }
                    }
                    else if (methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == WellKnownMethods.EnumerableContainsElement)
                    {
                        if (staticMethodCallExpression.Arguments is [_, var secondArgument])
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
