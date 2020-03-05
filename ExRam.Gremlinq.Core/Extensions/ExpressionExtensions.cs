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
        public static Expression StripConvert(this Expression expression)
        {
            while (true)
            {
                if (expression is UnaryExpression unaryExpression && expression.NodeType == ExpressionType.Convert)
                    expression = unaryExpression.Operand;
                else
                    return expression;
            }
        }

        public static Expression StripStepLabelValue(this Expression expression)
        {
            return expression is MemberExpression memberExpression && memberExpression.IsStepLabelValue()
                ? memberExpression.Expression
                : expression;
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
                expression = expression.StripConvert();

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

        public static bool IsPropertyValue(this MemberExpression expression)
        {
            return typeof(Property).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(Property<object>.Value);
        }

        public static bool IsPropertyKey(this MemberExpression expression)
        {
            return typeof(Property).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(Property<object>.Key);
        }

        public static bool IsStepLabelValue(this MemberExpression expression)
        {
            return typeof(StepLabel).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(StepLabel<object>.Value);
        }

        public static bool IsVertexPropertyLabel(this MemberExpression expression)
        {
            return typeof(IVertexProperty).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(VertexProperty<object>.Label);
        }

        public static bool IsVertexPropertyProperties(this MemberExpression expression)
        {
            return typeof(IVertexProperty).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(VertexProperty<object>.Properties);
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
                        var left = binaryExpression.Left.StripConvert();
                        var right = binaryExpression.Right.StripConvert();

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
                            var thisExpression = methodCallExpression.Arguments[0]
                                .StripConvert()
                                .StripStepLabelValue();

                            if (methodInfo.IsEnumerableAny())
                            {
                                if (thisExpression is MethodCallExpression previousMethodCallExpression && previousMethodCallExpression.Method.IsEnumerableIntersect())
                                {
                                    thisExpression = previousMethodCallExpression.Arguments[0]
                                        .StripConvert()
                                        .StripStepLabelValue();

                                    var argumentExpression = previousMethodCallExpression.Arguments[1]
                                        .StripConvert()
                                        .StripStepLabelValue();

                                    return argumentExpression.RefersToParameter()
                                        ? new GremlinExpression(argumentExpression, thisExpression.ToPWithin())
                                        : new GremlinExpression(thisExpression, argumentExpression.ToPWithin());
                                }

                                return new GremlinExpression(thisExpression, P.Neq(new object[] { null }));
                            }

                            if (methodInfo.IsEnumerableContains())
                            {
                                var argumentExpression = methodCallExpression.Arguments[1]
                                    .StripConvert()
                                    .StripStepLabelValue();

                                return argumentExpression.RefersToParameter()
                                    ? new GremlinExpression(argumentExpression, thisExpression.ToPWithin())
                                    : new GremlinExpression(thisExpression, P.Eq(argumentExpression));
                            }
                        }
                        else if (methodInfo.IsStringStartsWith() || methodInfo.IsStringEndsWith() || methodInfo.IsStringContains())
                        {
                            var instanceExpression = methodCallExpression.Object
                                .StripConvert()
                                .StripStepLabelValue();

                            var argumentExpression = methodCallExpression.Arguments[0]
                                .StripConvert()
                                .StripStepLabelValue();

                            if (methodInfo.IsStringStartsWith() && argumentExpression is MemberExpression)
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
                                            : methodInfo.IsStringStartsWith()
                                                ? TextP.StartingWith(str)
                                                : methodInfo.IsStringContains()
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

        internal static P ToPWithin(this Expression expression)
        {
            return expression.GetValue() switch
            {
                IEnumerable enumerable => P.Within(enumerable.Cast<object>().ToArray()),
                StepLabel stepLabel => P.Within(stepLabel),
                _ => throw new ExpressionNotSupportedException(expression)
            };
        }

        internal static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            if (expression.Body.StripConvert() is MemberExpression memberExpression)
                return memberExpression.Member;

            throw new ExpressionNotSupportedException(expression);
        }
    }
}
