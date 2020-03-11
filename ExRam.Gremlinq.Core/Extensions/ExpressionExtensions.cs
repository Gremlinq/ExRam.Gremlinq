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

        public static bool TryParseStepLabelExpression(this Expression expression, out StepLabel stepLabel, out MemberExpression? stepLabelValueMemberExpression)
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
            try
            {
                switch (body)
                {
                    case MemberExpression memberExpression when memberExpression.RefersToParameter():
                    {
                        if (memberExpression.Member is PropertyInfo property && property.PropertyType == typeof(bool))
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(memberExpression),
                                ExpressionSemantics.Equals,
                                ExpressionFragment.True);
                        }

                        break;
                    }
                    case BinaryExpression binaryExpression:
                    {
                        if (binaryExpression.NodeType != ExpressionType.AndAlso && binaryExpression.NodeType != ExpressionType.OrElse)
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(binaryExpression.Left.Strip()),
                                binaryExpression.NodeType.ToSemantics(),
                                ExpressionFragment.Create(binaryExpression.Right.Strip()));
                        }

                        break;
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        var pattern = methodCallExpression.TryGetWellKnownMember();
                        var thisExpression = methodCallExpression.Arguments[0].Strip();

                        switch (pattern)
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
                                var argumentExpression = methodCallExpression.Arguments[1].Strip();

                                return new GremlinExpression(
                                    ExpressionFragment.Create(thisExpression),
                                    ExpressionSemantics.Contains,
                                    ExpressionFragment.Create(argumentExpression));
                            }
                            case WellKnownMember.StringStartsWith:
                            case WellKnownMember.StringEndsWith:
                            case WellKnownMember.StringContains:
                            {
                                var instanceExpression = methodCallExpression.Object.Strip();
                                var argumentExpression = methodCallExpression.Arguments[0].Strip();

                                if (pattern == WellKnownMember.StringStartsWith && argumentExpression is MemberExpression)
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
                                    if (argumentExpression.GetValue() is string str)
                                    {
                                        return new GremlinExpression(
                                            ExpressionFragment.Create(instanceExpression),
                                            pattern switch
                                            {
                                                WellKnownMember.StringStartsWith => ExpressionSemantics.StartsWith,
                                                WellKnownMember.StringContains => ExpressionSemantics.HasInfix,
                                                WellKnownMember.StringEndsWith => ExpressionSemantics.EndsWith,
                                                _ => throw new ArgumentOutOfRangeException()
                                            },
                                            new ConstantExpressionFragment(str));
                                    }
                                }

                                break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(body, ex);
            }

            return default;
        }

        public static WellKnownMember? TryGetWellKnownMember(this Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                if (typeof(Property).IsAssignableFrom(memberExpression.Member.DeclaringType) && memberExpression.Member.Name == nameof(Property<object>.Value))
                    return WellKnownMember.PropertyValue;

                if (typeof(Property).IsAssignableFrom(memberExpression.Member.DeclaringType) && memberExpression.Member.Name == nameof(Property<object>.Key))
                    return WellKnownMember.PropertyKey;

                if (typeof(StepLabel).IsAssignableFrom(memberExpression.Member.DeclaringType) && memberExpression.Member.Name == nameof(StepLabel<object>.Value))
                    return WellKnownMember.StepLabelValue;

                if (typeof(IVertexProperty).IsAssignableFrom(memberExpression.Member.DeclaringType) && memberExpression.Member.Name == nameof(VertexProperty<object>.Label))
                    return WellKnownMember.VertexPropertyLabel;
            }
            else if (expression is MethodCallExpression methodCallExpression)
            {
                var methodInfo = methodCallExpression.Method;

                if (methodInfo.IsStatic)
                {
                    var thisExpression = methodCallExpression.Arguments[0].Strip();

                    if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericMethodDefinition() == EnumerableAny)
                    {
                        return thisExpression is MethodCallExpression previousMethodCallExpression && (previousMethodCallExpression is MethodCallExpression methodCallExpression3 && methodCallExpression3.Method.IsGenericMethod && (methodCallExpression3.Method.GetGenericMethodDefinition() == EnumerableIntersect))
                            ? WellKnownMember.EnumerableIntersectAny
                            : WellKnownMember.EnumerableAny;
                    }

                    if (methodCallExpression.Method.IsGenericMethod && (methodCallExpression.Method.GetGenericMethodDefinition() == EnumerableContainsElement))
                        return WellKnownMember.EnumerableContains;
                }
                else if (methodCallExpression.Method == StringStartsWith)
                    return WellKnownMember.StringStartsWith;
                else if (methodCallExpression.Method == StringEndsWith)
                    return WellKnownMember.StringEndsWith;
                else if (methodCallExpression.Method == StringContains)
                    return WellKnownMember.StringContains;
            }

            return null;
        }

        public static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            if (expression.Body.Strip() is MemberExpression memberExpression)
                return memberExpression.Member;

            throw new ExpressionNotSupportedException(expression);
        }

        private static MethodInfo Get(Expression<Action> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}
