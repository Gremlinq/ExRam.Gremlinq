using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionExtensions
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        private static readonly MethodInfo EnumerableAny = Get(() => Enumerable.Any<object>(default!)).GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableIntersect = Get(() => Enumerable.Intersect<object>(default!, default!)).GetGenericMethodDefinition()!;
#pragma warning disable 8625
        private static readonly MethodInfo EnumerableContainsElement = Get(() => Enumerable.Contains<object>(default!, default)).GetGenericMethodDefinition()!;
#pragma warning restore 8625
        // ReSharper disable once RedundantTypeSpecificationInDefaultExpression
        private static readonly MethodInfo StringStartsWith = Get(() => string.Empty.StartsWith(string.Empty));
        private static readonly MethodInfo StringContains = Get(() => string.Empty.Contains(string.Empty));
        private static readonly MethodInfo StringEndsWith = Get(() => string.Empty.EndsWith(string.Empty));
        // ReSharper disable once StringCompareToIsCultureSpecific
        private static readonly MethodInfo StringCompareTo = Get(() => string.Empty.CompareTo(string.Empty));
        // ReSharper restore ReturnValueOfPureMethodIsNotUsed

        private static readonly ExpressionSemantics[][] CompareToMatrix = {
            new [] { ExpressionSemantics.False, ExpressionSemantics.False,              ExpressionSemantics.LowerThan,          ExpressionSemantics.LowerThanOrEqual, ExpressionSemantics.True },
            new [] { ExpressionSemantics.False, ExpressionSemantics.LowerThan,          ExpressionSemantics.LowerThanOrEqual,   ExpressionSemantics.True,             ExpressionSemantics.True },
            new [] { ExpressionSemantics.False, ExpressionSemantics.LowerThan,          ExpressionSemantics.Equals,             ExpressionSemantics.GreaterThan,      ExpressionSemantics.False },
            new [] { ExpressionSemantics.True,  ExpressionSemantics.True,               ExpressionSemantics.GreaterThanOrEqual, ExpressionSemantics.GreaterThan,      ExpressionSemantics.False },
            new [] { ExpressionSemantics.True,  ExpressionSemantics.GreaterThanOrEqual, ExpressionSemantics.GreaterThan,        ExpressionSemantics.False,            ExpressionSemantics.False }
        };

        public static Expression Strip(this Expression expression)
        {
            while (true)
            {
                switch (expression)
                {
                    case UnaryExpression unaryExpression when expression.NodeType == ExpressionType.Convert:
                    {
                        expression = unaryExpression.Operand;
                        break;
                    }
                    default:
                    {
                        return expression;
                    }
                }
            }
        }

        public static object? GetValue(this Expression expression)
        {
            return expression switch
            {
                ConstantExpression constantExpression => constantExpression.Value,
                MethodCallExpression methodCallExpression => methodCallExpression.Method.Invoke(
                    methodCallExpression.Object?.GetValue(),
                    methodCallExpression.Arguments.Count > 0
                        ? methodCallExpression.Arguments.Select(argument => argument.GetValue()).ToArray()
                        : Array.Empty<object>()),
                MemberExpression { Member: PropertyInfo propertyInfo } propertyExpression => propertyInfo.GetValue(propertyExpression.Expression?.GetValue()),
                MemberExpression { Member: FieldInfo fieldInfo } fieldExpression => fieldInfo.GetValue(fieldExpression.Expression?.GetValue()),
                LambdaExpression lambdaExpression when lambdaExpression.Parameters.Count == 0 => lambdaExpression.Compile().DynamicInvoke(),
                _ => Expression.Lambda<Func<object>>(expression.Type.IsClass ? expression : Expression.Convert(expression, typeof(object))).Compile()()
            };
        }

        public static bool TryParseStepLabelExpression(this Expression expression, out StepLabel? stepLabel, out MemberExpression? stepLabelValueMemberExpression)
        {
            stepLabel = null;
            stepLabelValueMemberExpression = null;

            if (typeof(StepLabel).IsAssignableFrom(expression.Type))
            {
                stepLabel = (StepLabel?)expression.GetValue();

                return true;
            }

            if (expression is MemberExpression outerMemberExpression)
            {
                if (outerMemberExpression.TryGetWellKnownMember() == WellKnownMember.StepLabelValue)
                {
                    stepLabel = (StepLabel?)outerMemberExpression.Expression?.GetValue();

                    return true;
                }

                if (outerMemberExpression.Expression is MemberExpression innerMemberExpression && innerMemberExpression.TryGetWellKnownMember() == WellKnownMember.StepLabelValue)
                {
                    stepLabelValueMemberExpression = outerMemberExpression;
                    stepLabel = (StepLabel?)innerMemberExpression.Expression?.GetValue();

                    return true;
                }
            }

            return false;
        }

        public static ParameterExpression? TryGetReferredParameter(this Expression expression)
        {
            var actualExpression = (Expression?)expression;

            while (true)
            {
                if (actualExpression is null)
                    break;

                actualExpression = actualExpression.Strip();

                switch (actualExpression)
                {
                    case ParameterExpression parameterExpression:
                    {
                        return parameterExpression;
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

            return null;
        }

        public static bool IsIdentityExpression(this LambdaExpression expression)
        {
            return expression.Parameters.Count == 1 && expression.Body.Strip() == expression.Parameters[0];
        }

        public static GremlinExpression? TryToGremlinExpression(this Expression body, IGraphModel model)
        {
            var maybeExpression = body.TryToGremlinExpressionImpl(model);

            if (maybeExpression is { } expression)
            {
                if (expression.Left.Expression is MethodCallExpression leftMethodCallExpression)
                {
                    if (expression.LeftWellKnownMember == WellKnownMember.StringCompareTo && Convert.ToInt32(expression.Right.GetValue()) is { } comparison)
                    {
                        var semantics = CompareToMatrix[(int)expression.Semantics - 2][Math.Min(2, Math.Max(-2, comparison)) + 2];

                        return semantics switch
                        {
                            ExpressionSemantics.True => GremlinExpression.True,
                            ExpressionSemantics.False => GremlinExpression.False,
                            _ => new GremlinExpression(
                                ExpressionFragment.Create(leftMethodCallExpression.Object!, model),
                                default,
                                semantics,
                                ExpressionFragment.Create(leftMethodCallExpression.Arguments[0], model))
                        };
                    }
                }
                else if (expression.Left.Expression is UnaryExpression unaryExpression)
                {
                    if (unaryExpression.NodeType == ExpressionType.ArrayLength)
                    {
                        return new GremlinExpression(
                            ExpressionFragment.Create(unaryExpression.Operand, model),
                            WellKnownMember.ArrayLength,
                            expression.Semantics,
                            expression.Right);
                    }
                }
                else if (expression.Left.Expression is MemberExpression {Expression: {} memberExpressionExpression} && expression.LeftWellKnownMember != null)
                {
                    return new GremlinExpression(
                        ExpressionFragment.Create(memberExpressionExpression.Strip(), model),
                        expression.LeftWellKnownMember,
                        expression.Semantics,
                        expression.Right);
                }
            }

            return maybeExpression;
        }

        private static GremlinExpression? TryToGremlinExpressionImpl(this Expression body, IGraphModel model)
        {
            switch (body)
            {
                case MemberExpression memberExpression when memberExpression.TryGetReferredParameter() is not null && memberExpression.Member is PropertyInfo property && property.PropertyType == typeof(bool):
                {
                    return new GremlinExpression(
                        ExpressionFragment.Create(memberExpression, model),
                        default,
                        ExpressionSemantics.Equals,
                        ExpressionFragment.True);
                }
                case BinaryExpression binaryExpression when binaryExpression.NodeType != ExpressionType.AndAlso && binaryExpression.NodeType != ExpressionType.OrElse:
                {
                    return new GremlinExpression(
                        ExpressionFragment.Create(binaryExpression.Left.Strip(), model),
                        default,
                        binaryExpression.NodeType.ToSemantics(),
                        ExpressionFragment.Create(binaryExpression.Right.Strip(), model));
                }
                case MethodCallExpression methodCallExpression:
                {
                    var wellKnownMember = methodCallExpression.TryGetWellKnownMember();

                    switch (wellKnownMember)
                    {
                        case WellKnownMember.EnumerableIntersectAny:
                        {
                            var arguments = ((MethodCallExpression)methodCallExpression.Arguments[0].Strip()).Arguments;

                            return new GremlinExpression(
                                ExpressionFragment.Create(arguments[0].Strip(), model),
                                default,
                                ExpressionSemantics.Intersects,
                                ExpressionFragment.Create(arguments[1].Strip(), model));
                        }
                        case WellKnownMember.EnumerableAny:
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(methodCallExpression.Arguments[0].Strip(), model),
                                default,
                                ExpressionSemantics.NotEquals,
                                ExpressionFragment.Null);
                        }
                        case WellKnownMember.EnumerableContains:
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(methodCallExpression.Arguments[0].Strip(), model),
                                default,
                                ExpressionSemantics.Contains,
                                ExpressionFragment.Create(methodCallExpression.Arguments[1].Strip(), model));
                        }
                        case WellKnownMember.ListContains:
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(methodCallExpression.Object!, model),
                                default,
                                ExpressionSemantics.Contains,
                                ExpressionFragment.Create(methodCallExpression.Arguments[0].Strip(), model));
                        }
                        case WellKnownMember.StringStartsWith:
                        case WellKnownMember.StringEndsWith:
                        case WellKnownMember.StringContains:
                        {
                            var instanceExpression = methodCallExpression.Object!.Strip();
                            var argumentExpression = methodCallExpression.Arguments[0].Strip();

                            if (wellKnownMember == WellKnownMember.StringStartsWith && argumentExpression.TryGetReferredParameter() is not null)
                            {
                                if (instanceExpression.GetValue() is string stringValue)
                                {
                                    return new GremlinExpression(
                                        ExpressionFragment.Constant(stringValue),
                                        default,
                                        ExpressionSemantics.StartsWith,
                                        ExpressionFragment.Create(argumentExpression, model));
                                }
                            }
                            else if (instanceExpression.TryGetReferredParameter() is not null)
                            {
                                if (argumentExpression.GetValue() is string stringValue)
                                {
                                    return new GremlinExpression(
                                        ExpressionFragment.Create(instanceExpression, model),
                                        default,
                                        wellKnownMember switch
                                        {
                                            WellKnownMember.StringStartsWith => ExpressionSemantics.StartsWith,
                                            WellKnownMember.StringContains => ExpressionSemantics.HasInfix,
                                            WellKnownMember.StringEndsWith => ExpressionSemantics.EndsWith,
                                            _ => throw new ExpressionNotSupportedException(methodCallExpression)
                                        },
                                        ExpressionFragment.Constant(stringValue));
                                }
                            }

                            break;
                        }
                    }

                    break;
                }
            }

            return default;
        }

        public static WellKnownMember? TryGetWellKnownMember(this Expression expression)
        {
            switch (expression)
            {
                case MemberExpression memberExpression:
                {
                    var member = memberExpression.Member;

                    if (typeof(IProperty).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(IProperty.Value))
                        return WellKnownMember.PropertyValue;

                    if (typeof(IProperty).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(Property<object>.Key))
                        return WellKnownMember.PropertyKey;

                    if (typeof(StepLabel).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(StepLabel<object>.Value))
                        return WellKnownMember.StepLabelValue;

                    if (typeof(IVertexProperty).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(VertexProperty<object>.Label))
                        return WellKnownMember.VertexPropertyLabel;

                    break;
                }
                case MethodCallExpression methodCallExpression:
                {
                    var methodInfo = methodCallExpression.Method;

                    if (methodInfo.IsStatic)
                    {
                        var thisExpression = methodCallExpression.Arguments[0].Strip();

                        if (methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == EnumerableAny)
                        {
                            return thisExpression is MethodCallExpression previousMethodCallExpression && previousMethodCallExpression.Method.IsGenericMethod && previousMethodCallExpression.Method.GetGenericMethodDefinition() == EnumerableIntersect
                                ? WellKnownMember.EnumerableIntersectAny
                                : WellKnownMember.EnumerableAny;
                        }

                        if (methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == EnumerableContainsElement)
                            return WellKnownMember.EnumerableContains;
                    }
                    else
                    {
                        if (typeof(IList).IsAssignableFrom(methodInfo.DeclaringType) && methodInfo.Name == nameof(List<object>.Contains))
                            return WellKnownMember.ListContains;

                        if (methodInfo == StringStartsWith)
                            return WellKnownMember.StringStartsWith;

                        if (methodInfo == StringEndsWith)
                            return WellKnownMember.StringEndsWith;

                        if (methodInfo == StringContains)
                            return WellKnownMember.StringContains;

                        if (methodInfo == StringCompareTo)
                            return WellKnownMember.StringCompareTo;
                    }

                    break;
                }
            }

            return null;
        }

        private static MethodInfo Get(Expression<Action> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}
