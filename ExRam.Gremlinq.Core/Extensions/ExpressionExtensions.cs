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

        public static bool IsMemberOnStepLabelValue(this Expression expression)
        {
            return expression is MemberExpression valueMemberExpression
                && valueMemberExpression.Expression is MemberExpression valueExpression
                && valueExpression.IsStepLabelValue();
        }

        public static bool HasExpressionInMemberChain(this Expression expression, Expression searchedExpression)
        {
            while (true)
            {
                expression = expression.StripConvert();

                if (expression == searchedExpression)
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

            return expression.Body.TryToGremlinExpression(expression.Parameters[0]);
        }

        public static GremlinExpression? TryToGremlinExpression(this Expression body, ParameterExpression parameter)
        {
            try
            {
                switch (body)
                {
                    case MemberExpression memberExpression when memberExpression.HasExpressionInMemberChain(parameter):
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
                            if (left.TryToGremlinExpression(parameter) is { } leftExpression && right.TryToGremlinExpression(parameter) is { } rightExpression)
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
                            var parameterIsInRight = right.HasExpressionInMemberChain(parameter);
                            var parameterIsInLeft = left.HasExpressionInMemberChain(parameter);

                            if (parameterIsInRight && !parameterIsInLeft)
                                return new GremlinExpression(right, binaryExpression.NodeType.Switch().ToP(left));

                            if (parameterIsInLeft && !parameterIsInRight)
                                return new GremlinExpression(left, binaryExpression.NodeType.ToP(right));
                        }

                        break;
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        var methodInfo = methodCallExpression.Method;

                        if (methodInfo.IsEnumerableAny())
                        {
                            if (methodCallExpression.Arguments[0] is MethodCallExpression previousExpression && previousExpression.Method.IsEnumerableIntersect())
                            {
                                if (previousExpression.Arguments[0] is MemberExpression sourceMember)
                                    return new GremlinExpression(sourceMember, new P("within", previousExpression.Arguments[1].GetValue()));

                                if (previousExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                    return new GremlinExpression(argument, new P("within", previousExpression.Arguments[0].GetValue()));
                            }
                            else
                                return new GremlinExpression(methodCallExpression.Arguments[0], P.Neq(new object[] { null }));
                        }
                        else if (methodInfo.IsEnumerableContains() || methodInfo.IsStepLabelContains())
                        {
                            var methodCallArgument0 = methodCallExpression.Arguments[0].StripConvert();
                            var methodCallArgument1 = methodCallExpression.Arguments[1].StripConvert();

                            if (methodCallArgument0 is MemberExpression sourceMember && sourceMember.Expression == parameter)
                                return new GremlinExpression(sourceMember, P.Eq(methodCallArgument1));

                            if (methodCallArgument1 is MemberExpression argument && argument.Expression == parameter)
                                return new GremlinExpression(argument, new P("within", methodCallArgument0.GetValue()));

                            if (methodCallArgument1 == parameter)
                                return new GremlinExpression(parameter, new P("within", methodCallArgument0.GetValue()));
                        }
                        else if (methodInfo.IsStringStartsWith() || methodInfo.IsStringEndsWith() || methodInfo.IsStringContains())
                        {
                            var methodCallExpressionObject = methodCallExpression.Object.StripConvert();
                            var methodCallArgument = methodCallExpression.Arguments[0].StripConvert();

                            if (methodInfo.IsStringStartsWith() && methodCallArgument is MemberExpression argumentExpression)
                            {
                                if (methodCallExpressionObject.GetValue() is string stringValue)
                                {
                                    return new GremlinExpression(
                                        argumentExpression,
                                        P.Within(Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => stringValue.Substring(0, i))
                                            .ToArray<object>()));
                                }
                            }
                            else if (methodCallExpressionObject is MemberExpression memberExpression)
                            {
                                if (methodCallArgument.GetValue() is string str)
                                {
                                    return new GremlinExpression(
                                        memberExpression,
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

        internal static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            if (expression.Body.StripConvert() is MemberExpression memberExpression)
                return memberExpression.Member;

            throw new ExpressionNotSupportedException(expression);
        }
    }
}
