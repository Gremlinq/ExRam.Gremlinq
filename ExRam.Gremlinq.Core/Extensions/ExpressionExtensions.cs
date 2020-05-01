using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionExtensions
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        private static readonly MethodInfo EnumerableAny = Get(() => Enumerable.Any<object>(default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableIntersect = Get(() => Enumerable.Intersect<object>(default, default))?.GetGenericMethodDefinition()!;
#pragma warning disable 8625
        private static readonly MethodInfo EnumerableContainsElement = Get(() => Enumerable.Contains<object>(default, default))?.GetGenericMethodDefinition()!;
#pragma warning restore 8625
        // ReSharper disable once RedundantTypeSpecificationInDefaultExpression
        private static readonly MethodInfo StringStartsWith = Get(() => string.Empty.StartsWith(string.Empty));
        private static readonly MethodInfo StringContains = Get(() => string.Empty.Contains(string.Empty));
        private static readonly MethodInfo StringEndsWith = Get(() => string.Empty.EndsWith(string.Empty));
        // ReSharper restore ReturnValueOfPureMethodIsNotUsed

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
                    case MemberExpression memberExpression when memberExpression.TryGetWellKnownMember() == WellKnownMember.StepLabelValue:
                    {
                        return memberExpression.Expression;
                    }
                    default:
                    {
                        return expression;
                    }
                }
            }
        }

        public static object GetValue(this Expression expression)
        {
            return expression switch
            {
                ConstantExpression constantExpression => constantExpression.Value,
                MemberExpression memberExpression when memberExpression.Member is FieldInfo fieldInfo && memberExpression.Expression is ConstantExpression constant => fieldInfo.GetValue(constant.Value),
                LambdaExpression lambdaExpression => lambdaExpression.Compile().DynamicInvoke(),
                _ => Expression.Lambda<Func<object>>(expression.Type.IsClass ? expression : Expression.Convert(expression, typeof(object))).Compile()()
            };
        }

        public static bool TryParseStepLabelExpression(this Expression expression, out StepLabel? stepLabel, out MemberExpression? stepLabelValueMemberExpression)
        {
            stepLabel = null;
            stepLabelValueMemberExpression = null;

            if (typeof(StepLabel).IsAssignableFrom(expression.Type))
            {
                stepLabel = (StepLabel)expression.GetValue();

                return true;
            }

            if (expression is MemberExpression outerMemberExpression)
            {
                if (outerMemberExpression.TryGetWellKnownMember() == WellKnownMember.StepLabelValue)
                {
                    stepLabel = (StepLabel)outerMemberExpression.Expression.GetValue();

                    return true;
                }

                stepLabelValueMemberExpression = outerMemberExpression;

                if (outerMemberExpression.Expression is MemberExpression innerMemberExpression)
                {
                    if (innerMemberExpression.TryGetWellKnownMember() == WellKnownMember.StepLabelValue)
                    {
                        stepLabel = (StepLabel)innerMemberExpression.Expression.GetValue();

                        return true;
                    }
                }
            }

            return false;
        }

        public static bool RefersToParameter(this Expression expression)
        {
            while (true)
            {
                expression = expression.Strip();

                switch (expression)
                {
                    case ParameterExpression _:
                    {
                        return true;
                    }
                    case LambdaExpression lambdaExpression:
                    {
                        expression = lambdaExpression.Body;
                        break;
                    }
                    case MemberExpression memberExpression:
                    {
                        expression = memberExpression.Expression;
                        break;
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        expression = methodCallExpression.Object;
                        break;
                    }
                    default:
                    {
                        return false;
                    }
                }
            }
        }

        public static GremlinExpression? TryToGremlinExpression(this Expression body)
        {
            switch (body)
            {
                case MemberExpression memberExpression when memberExpression.RefersToParameter() && memberExpression.Member is PropertyInfo property && property.PropertyType == typeof(bool):
                {
                    return new GremlinExpression(
                        ExpressionFragment.Create(memberExpression),
                        ExpressionSemantics.Equals,
                        ExpressionFragment.True);
                }
                case BinaryExpression binaryExpression when binaryExpression.NodeType != ExpressionType.AndAlso && binaryExpression.NodeType != ExpressionType.OrElse:
                {
                    return new GremlinExpression(
                        ExpressionFragment.Create(binaryExpression.Left.Strip()),
                        binaryExpression.NodeType.ToSemantics(),
                        ExpressionFragment.Create(binaryExpression.Right.Strip()));
                }
                case MethodCallExpression methodCallExpression:
                {
                    var wellKnownMember = methodCallExpression.TryGetWellKnownMember();
                    var thisExpression = methodCallExpression.Arguments[0].Strip();

                    switch (wellKnownMember)
                    {
                        case WellKnownMember.EnumerableIntersectAny:
                        {
                            var arguments = ((MethodCallExpression)thisExpression).Arguments;

                            return new GremlinExpression(
                                ExpressionFragment.Create(arguments[0].Strip()),
                                ExpressionSemantics.Intersects,
                                ExpressionFragment.Create(arguments[1].Strip()));
                        }
                        case WellKnownMember.EnumerableAny:
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(thisExpression),
                                ExpressionSemantics.NotEquals,
                                ExpressionFragment.Null);
                        }
                        case WellKnownMember.EnumerableContains:
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(thisExpression),
                                ExpressionSemantics.Contains,
                                ExpressionFragment.Create(methodCallExpression.Arguments[1].Strip()));
                        }
                        case WellKnownMember.StringStartsWith:
                        case WellKnownMember.StringEndsWith:
                        case WellKnownMember.StringContains:
                        {
                            var instanceExpression = methodCallExpression.Object.Strip();
                            var argumentExpression = methodCallExpression.Arguments[0].Strip();

                            if (wellKnownMember == WellKnownMember.StringStartsWith && argumentExpression is MemberExpression)
                            {
                                if (instanceExpression.GetValue() is string stringValue)
                                {
                                    return new GremlinExpression(
                                        new ConstantExpressionFragment(stringValue),
                                        ExpressionSemantics.StartsWith,
                                        ExpressionFragment.Create(argumentExpression));
                                }
                            }
                            else if (instanceExpression is MemberExpression)
                            {
                                if (argumentExpression.GetValue() is string stringValue)
                                {
                                    return new GremlinExpression(
                                        ExpressionFragment.Create(instanceExpression),
                                        wellKnownMember switch
                                        {
                                            WellKnownMember.StringStartsWith => ExpressionSemantics.StartsWith,
                                            WellKnownMember.StringContains => ExpressionSemantics.HasInfix,
                                            WellKnownMember.StringEndsWith => ExpressionSemantics.EndsWith,
                                            _ => throw new ArgumentOutOfRangeException()
                                        },
                                        new ConstantExpressionFragment(stringValue));
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

                    if (typeof(Property).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(Property<object>.Value))
                        return WellKnownMember.PropertyValue;

                    if (typeof(Property).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(Property<object>.Key))
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
                    else if (methodInfo == StringStartsWith)
                        return WellKnownMember.StringStartsWith;
                    else if (methodInfo == StringEndsWith)
                        return WellKnownMember.StringEndsWith;
                    else if (methodInfo == StringContains)
                        return WellKnownMember.StringContains;

                    break;
                }
            }

            return null;
        }

        public static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            return expression.Body.Strip() is MemberExpression memberExpression
                ? memberExpression.Member
                : throw new ExpressionNotSupportedException(expression);
        }

        private static MethodInfo Get(Expression<Action> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}
