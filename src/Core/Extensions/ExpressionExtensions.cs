using System.Collections.Concurrent;using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using ExRam.Gremlinq.Core.ExpressionParsing;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionExtensions
    {
        private sealed record NumberOperators(Func<object, object> Negate, Func<object, object, object> Add, Func<object, object, object> Subtract, Func<object, object, object> Multiply, Func<object, object, object> Divide, Func<object, object, object> Modulo)
        {
            public object Apply(BinaryExpression binaryExpression)
            {
                var left = binaryExpression.Left.GetValue()!;
                var right = binaryExpression.Right.GetValue()!;

                return binaryExpression.NodeType switch
                {
                    ExpressionType.Add => Add(left, right),
                    ExpressionType.Subtract => Subtract(left, right),
                    ExpressionType.Multiply => Multiply(left, right),
                    ExpressionType.Divide => Divide(left, right),
                    _ => Modulo(left, right)
                };
            }
        }

        private static class Number<T>
            where T : INumber<T>
        {
            public static readonly NumberOperators Operators = new(
                static value => -(T)value,
                static (left, right) => (T)left + (T)right,
                static (left, right) => (T)left - (T)right,
                static (left, right) => (T)left * (T)right,
                static (left, right) => (T)left / (T)right,
                static (left, right) => (T)left % (T)right);
        }

        private static readonly ConcurrentDictionary<Type, NumberOperators?> NumberOperatorsByType = new();

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
                    case MethodCallExpression { Object: { } objectExpression } methodCallExpression when methodCallExpression.Method == WellKnownMethods.ObjectToString && objectExpression.RefersToParameter(out _):
                    {
                        expression = objectExpression;
                        break;
                    }
                    case MethodCallExpression { Object: null, Method: { DeclaringType: { } declaringType } method, Arguments: [{ } singleArgument] } when declaringType.IsSpanType() && method is { IsSpecialName: true, Name: "op_Implicit" or "op_Explicit" }:
                    {
                        expression = singleArgument;
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

        public static object? GetValue(this Expression expression) => expression switch
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
            UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked, Method: null } convertExpression => convertExpression.GetConvertedValue(),
            UnaryExpression { NodeType: ExpressionType.Negate, Method: null } negateExpression when GetNumberOperators(negateExpression.Type) is { } operators => operators.Negate(negateExpression.Operand.GetValue()!),
            ConditionalExpression conditionalExpression => (bool)conditionalExpression.Test.GetValue()!
                ? conditionalExpression.IfTrue.GetValue()
                : conditionalExpression.IfFalse.GetValue(),
            BinaryExpression { NodeType: ExpressionType.ArrayIndex, Left: { } arrayExpression, Right: { } indexExpression } => ((Array)arrayExpression.GetValue()!).GetValue((int)indexExpression.GetValue()!),
            BinaryExpression { NodeType: ExpressionType.Add or ExpressionType.Subtract or ExpressionType.Multiply or ExpressionType.Divide or ExpressionType.Modulo, Method: null } binaryExpression when GetNumberOperators(binaryExpression.Type) is { } operators => operators.Apply(binaryExpression),
            _ => expression.Interpret()
        };

        // The operators of a number type, closed over the type once per type the parser meets: INumber<T> gives every
        // number type the unchecked operators of Expression.Add and the others. A type that is no number has none.
        private static NumberOperators? GetNumberOperators(Type type) => NumberOperatorsByType.GetOrAdd(
            type,
            static type => type.IsValueType && type.GetInterfaces().Any(implementedInterface => implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == typeof(INumber<>) && implementedInterface.GetGenericArguments()[0] == type)
                ? (NumberOperators?)typeof(Number<>).MakeGenericType(type).GetField(nameof(Number<int>.Operators), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)
                : null);

        // A conversion that boxes the value, widens it to a base type or an interface or wraps it into a nullable
        // leaves the object as it is. Every other conversion, as of a number to a wider number or of an enum to its
        // underlying type, is interpreted on the value already read, so that the operand is read once.
        private static object? GetConvertedValue(this UnaryExpression convertExpression)
        {
            var maybeValue = convertExpression.Operand.GetValue();

            return maybeValue is { } value && convertExpression.Type.IsAssignableFrom(value.GetType())
                ? value
                : Expression
                    .MakeUnary(convertExpression.NodeType, Expression.Constant(maybeValue, convertExpression.Operand.Type), convertExpression.Type)
                    .Interpret();
        }

        // The interpreter runs an expression without emitting and jitting a method for it, which is what
        // Compile() did for every query whose constant was not read directly.
        private static object? Interpret(this Expression expression) => Expression
            .Lambda<Func<object?>>(expression.Type.IsClass ? expression : Expression.Convert(expression, typeof(object)))
            .Compile(preferInterpretation: true)();

        public static bool RefersToStepLabel(this Expression expression, [NotNullWhen(true)] out StepLabel? stepLabel, out MemberExpression? stepLabelValueMemberExpression)
        {
            stepLabel = null;
            stepLabelValueMemberExpression = null;

            if (typeof(StepLabel).IsAssignableFrom(expression.Type))
                stepLabel = (StepLabel?)expression.GetValue();
            else if (expression is MemberExpression outerMemberExpression)
            {
                if (outerMemberExpression.IsStepLabelValue(out var stepLabelExpression))
                    stepLabel = (StepLabel?)stepLabelExpression.GetValue();
                else if (outerMemberExpression.Expression is MemberExpression innerMemberExpression && innerMemberExpression.IsStepLabelValue(out stepLabelExpression))
                {
                    stepLabelValueMemberExpression = outerMemberExpression;
                    stepLabel = (StepLabel?)stepLabelExpression.GetValue();
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
                    
                    return new WhereExpression(
                        binaryExpression.Left,
                        semantics,
                        binaryExpression.Right);
                }
                case MethodCallExpression methodCallExpression when methodCallExpression.LooksLikeCollectionContains(out var collection, out var containsArgument):
                {
                    return new WhereExpression(
                        collection,
                        ContainsExpressionSemantics.Instance,
                        containsArgument);
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

                    if (methodInfo.DeclaringType == typeof(string) && methodInfo.GetParameters() is { Length: 1 or 2 } parameters)
                    {
                        if ((parameters[0].ParameterType == typeof(string) || parameters[0].ParameterType == typeof(char)) && (parameters.Length == 1 || parameters[1].ParameterType == typeof(StringComparison)))
                        {
                            var instanceExpression = targetExpression.Strip();
                            var argumentExpression = firstArgument.Strip();

                            var stringComparison = instanceMethodCallExpression.Arguments is [_, { } secondArgument, ..] && secondArgument.Type == typeof(StringComparison)
                                ? (StringComparison)secondArgument.GetValue()!
                                : StringComparison.Ordinal;

                            if (argumentExpression.RefersToParameter(out _) && !instanceExpression.RefersToParameter(out _))
                            {
                                if (instanceExpression.GetValue()?.ToString() is { } stringValue)
                                {
                                    return new WhereExpression(
                                        argumentExpression,
                                        methodInfo.Name switch
                                        {
                                            nameof(string.StartsWith) => IsPrefixOfExpressionSemantics.Get(stringComparison),
                                            nameof(string.Contains) => IsInfixOfExpressionSemantics.Get(stringComparison),
                                            _ => throw new ExpressionNotSupportedException(instanceMethodCallExpression)
                                        },
                                        Expression.Constant(stringValue));
                                }
                            }
                            else if (instanceExpression.RefersToParameter(out _))
                            {
                                if (argumentExpression.GetValue()?.ToString() is { } stringValue)
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
                    
                    break;
                }
            }

            return null;
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

            return [];
        }
    }
}
