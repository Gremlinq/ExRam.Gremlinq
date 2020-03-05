using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionExtensions
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        private static readonly MethodInfo EnumerableAny = Get(() => Enumerable.Any<object>(default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableIntersect = Get(() => Enumerable.Intersect<object>(default, default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableContainsElement = Get(() => Enumerable.Contains<object>(default, default))?.GetGenericMethodDefinition()!;
        // ReSharper disable once RedundantTypeSpecificationInDefaultExpression
        private static readonly MethodInfo StringStartsWith = Get(() => string.Empty.StartsWith(default(string)));
        private static readonly MethodInfo StringContains = Get(() => string.Empty.Contains(default(string)));
        private static readonly MethodInfo StringEndsWith = Get(() => string.Empty.EndsWith(default(string)));
        // ReSharper restore ReturnValueOfPureMethodIsNotUsed

        public static Expression Strip(this Expression expression)
        {
            while (true)
            {
                if (expression is UnaryExpression unaryExpression && expression.NodeType == ExpressionType.Convert)
                    expression = unaryExpression.Operand;
                else if (expression is MemberExpression memberExpression && memberExpression.IsStepLabelValue())
                    return memberExpression.Expression;
                else
                    return expression;
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
                if (outerMemberExpression.IsStepLabelValue())
                {
                    stepLabel = (StepLabel)outerMemberExpression.Expression.GetValue();

                    return true;
                }

                stepLabelValueMemberExpression = outerMemberExpression;

                if (outerMemberExpression.Expression is MemberExpression innerMemberExpression)
                {
                    if (innerMemberExpression.IsStepLabelValue())
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

                if (expression is ParameterExpression)
                    return true;

                if (expression is MemberExpression memberExpression)
                    expression = memberExpression.Expression;
                else if (expression is MethodCallExpression methodCallExpression)
                    expression = methodCallExpression.Object;
                else
                    return false;
            }
        }

        public static GremlinExpression? TryToGremlinExpression(this LambdaExpression expression)
        {
            if (expression.Parameters.Count != 1)
                throw new ExpressionNotSupportedException(expression);

            return expression.Body.TryToGremlinExpression();
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
                            return new GremlinExpression(memberExpression, P.Eq(true));

                        break;
                    }
                    case BinaryExpression binaryExpression:
                    {
                        var left = binaryExpression.Left.Strip();
                        var right = binaryExpression.Right.Strip();

                        if (binaryExpression.NodeType == ExpressionType.AndAlso || binaryExpression.NodeType == ExpressionType.OrElse)
                        {
                            if (left.TryToGremlinExpression() is { } leftExpression && right.TryToGremlinExpression() is { } rightExpression)
                            {
                                if (leftExpression.Key == rightExpression.Key || leftExpression.Key is MemberExpression memberExpression1 && rightExpression.Key is MemberExpression memberExpression2 && memberExpression1.Member == memberExpression2.Member)
                                {
                                    return new GremlinExpression(
                                        leftExpression.Key,
                                        binaryExpression.NodeType switch
                                        {
                                            ExpressionType.AndAlso => leftExpression.Predicate.And(rightExpression.Predicate),
                                            ExpressionType.OrElse => leftExpression.Predicate.Or(rightExpression.Predicate),
                                            _ => throw new ExpressionNotSupportedException(body)
                                        });
                                }
                            }
                        }
                        else
                        {
                            return right.RefersToParameter() && !left.RefersToParameter()
                                ? new GremlinExpression(right, binaryExpression.NodeType.Switch().ToP(left))
                                : new GremlinExpression(left, binaryExpression.NodeType.ToP(right));
                        }

                        break;
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        var methodInfo = methodCallExpression.Method;

                        if (methodInfo.IsStatic)
                        {
                            var thisExpression = methodCallExpression.Arguments[0].Strip();

                            if (methodCallExpression.IsEnumerableAny())
                            {
                                if (thisExpression is MethodCallExpression previousMethodCallExpression && previousMethodCallExpression.IsEnumerableIntersect())
                                {
                                    thisExpression = previousMethodCallExpression.Arguments[0].Strip();
                                    var argumentExpression = previousMethodCallExpression.Arguments[1].Strip();

                                    return argumentExpression.RefersToParameter()
                                        ? new GremlinExpression(argumentExpression, thisExpression.ToPWithin())
                                        : new GremlinExpression(thisExpression, argumentExpression.ToPWithin());
                                }

                                return new GremlinExpression(thisExpression, P.Neq(new object[] { null }));
                            }

                            if (methodCallExpression.IsEnumerableContains())
                            {
                                var argumentExpression = methodCallExpression.Arguments[1].Strip();

                                return argumentExpression.RefersToParameter()
                                    ? new GremlinExpression(argumentExpression, thisExpression.ToPWithin())
                                    : new GremlinExpression(thisExpression, P.Eq(argumentExpression));
                            }
                        }
                        else if (methodCallExpression.IsStringStartsWith() || methodCallExpression.IsStringEndsWith() || methodCallExpression.IsStringContains())
                        {
                            var instanceExpression = methodCallExpression.Object.Strip();
                            var argumentExpression = methodCallExpression.Arguments[0].Strip();

                            if (methodCallExpression.IsStringStartsWith() && argumentExpression is MemberExpression)
                            {
                                if (instanceExpression.GetValue() is string stringValue)
                                {
                                    return new GremlinExpression(
                                        argumentExpression,
                                        P.Within(Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => stringValue.Substring(0, i))
                                            .ToArray<object>()));
                                }
                            }
                            else if (instanceExpression is MemberExpression)
                            {
                                if (argumentExpression.GetValue() is string str)
                                {
                                    return new GremlinExpression(
                                        instanceExpression,
                                        str.Length == 0
                                            ? P.Without(Array.Empty<object>())
                                            : methodCallExpression.IsStringStartsWith()
                                                ? TextP.StartingWith(str)
                                                : methodCallExpression.IsStringContains()
                                                    ? TextP.Containing(str)
                                                    : TextP.EndingWith(str));
                                }
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

        public static P ToPWithin(this Expression expression)
        {
            return expression.GetValue() switch
            {
                IEnumerable enumerable => P.Within(enumerable.Cast<object>().ToArray()),
                StepLabel stepLabel => P.Within(stepLabel),
                _ => throw new ExpressionNotSupportedException(expression)
            };
        }

        public static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            if (expression.Body.Strip() is MemberExpression memberExpression)
                return memberExpression.Member;

            throw new ExpressionNotSupportedException(expression);
        }

        public static bool IsPropertyValue(this Expression expression)
        {
            return expression is MemberExpression memberExpression && typeof(Property).IsAssignableFrom(memberExpression.Expression.Type) && memberExpression.Member.Name == nameof(Property<object>.Value);
        }

        public static bool IsPropertyKey(this Expression expression)
        {
            return expression is MemberExpression memberExpression && typeof(Property).IsAssignableFrom(memberExpression.Expression.Type) && memberExpression.Member.Name == nameof(Property<object>.Key);
        }

        public static bool IsStepLabelValue(this Expression expression)
        {
            return expression is MemberExpression memberExpression && typeof(StepLabel).IsAssignableFrom(memberExpression.Expression.Type) && memberExpression.Member.Name == nameof(StepLabel<object>.Value);
        }

        public static bool IsVertexPropertyLabel(this Expression expression)
        {
            return expression is MemberExpression memberExpression && typeof(IVertexProperty).IsAssignableFrom(memberExpression.Expression.Type) && memberExpression.Member.Name == nameof(VertexProperty<object>.Label);
        }

        public static bool IsEnumerableAny(this Expression expression)
        {
            return expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericMethodDefinition() == EnumerableAny;
        }

        public static bool IsEnumerableContains(this Expression expression)
        {
            return expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.IsGenericMethod && (methodCallExpression.Method.GetGenericMethodDefinition() == EnumerableContainsElement);
        }

        public static bool IsEnumerableIntersect(this Expression expression)
        {
            return expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.IsGenericMethod && (methodCallExpression.Method.GetGenericMethodDefinition() == EnumerableIntersect);
        }

        public static bool IsStringStartsWith(this Expression expression)
        {
            return expression is MethodCallExpression methodCallExpression && methodCallExpression.Method == StringStartsWith;
        }

        public static bool IsStringContains(this Expression expression)
        {
            return expression is MethodCallExpression methodCallExpression && methodCallExpression.Method == StringContains;
        }

        public static bool IsStringEndsWith(this Expression expression)
        {
            return expression is MethodCallExpression methodCallExpression && methodCallExpression.Method == StringEndsWith;
        }

        private static MethodInfo Get(Expression<Action> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}
