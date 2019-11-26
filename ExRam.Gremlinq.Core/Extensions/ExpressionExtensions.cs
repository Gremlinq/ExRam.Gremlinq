using System.Collections;
using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;

namespace System.Linq.Expressions
{
    internal static class ExpressionExtensions
    {
        public static Expression StripConvert(this Expression expression)
        {
            if (expression is UnaryExpression unaryExpression && expression.NodeType == ExpressionType.Convert)
            {
                // ReSharper disable once TailRecursiveCall
                return unaryExpression.Operand.StripConvert();
            }

            return expression;
        }

        public static object GetValue(this Expression expression)
        {
            if (expression is ConstantExpression constantExpression)
                return constantExpression.Value;

            return Expression
                .Lambda<Func<object>>(Expression.Convert(expression, typeof(object)))
                .Compile()();
        }

        public static bool HasExpressionInMemberChain(this Expression expression, Expression searchedExpression)
        {
            while (true)
            {
                if (expression == searchedExpression)
                    return true;

                if (expression is MemberExpression memberExpression)
                {
                    expression = memberExpression.Expression;
                    continue;
                }

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

        public static bool IsVertexPropertyLabel(this MemberExpression expression)
        {
            return typeof(IVertexProperty).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(VertexProperty<object>.Label);
        }

        public static bool IsVertexPropertyProperties(this MemberExpression expression)
        {
            return typeof(IVertexProperty).IsAssignableFrom(expression.Expression.Type) && expression.Member.Name == nameof(VertexProperty<object>.Properties);
        }

        public static GremlinExpression ToGremlinExpression(this LambdaExpression expression)
        {
            if (expression.Parameters.Count != 1)
                throw new ExpressionNotSupportedException(expression);

            return expression.Body.ToGremlinExpression(expression.Parameters[0]).Simplify();
        }

        private static GremlinExpression ToGremlinExpression(this Expression expression, Expression parameter)
        {
            try
            {
                switch (expression)
                {
                    case UnaryExpression unaryExpression:
                    {
                        if (unaryExpression.NodeType == ExpressionType.Not)
                            return unaryExpression.Operand.ToGremlinExpression(parameter).Negate();

                        break;
                    }
                    case MemberExpression memberExpression:
                    {
                        if (memberExpression.Member is PropertyInfo property && property.PropertyType == typeof(bool))
                            return new TerminalGremlinExpression(parameter, memberExpression, P.Eq(true));

                        break;
                    }
                    case BinaryExpression binaryExpression:
                    {
                        if (binaryExpression.NodeType == ExpressionType.AndAlso)
                            return new AndGremlinExpression(parameter, binaryExpression.Left.StripConvert().ToGremlinExpression(parameter), binaryExpression.Right.StripConvert().ToGremlinExpression(parameter));

                        if (binaryExpression.NodeType == ExpressionType.OrElse)
                            return new OrGremlinExpression(parameter, binaryExpression.Left.StripConvert().ToGremlinExpression(parameter), binaryExpression.Right.StripConvert().ToGremlinExpression(parameter));

                        return binaryExpression.Right.HasExpressionInMemberChain(parameter)
                            ? new TerminalGremlinExpression(parameter, binaryExpression.Right.StripConvert(), binaryExpression.NodeType.Switch().ToP(binaryExpression.Left.GetValue()))
                            : new TerminalGremlinExpression(parameter, binaryExpression.Left.StripConvert(), binaryExpression.NodeType.ToP(binaryExpression.Right.GetValue()));
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        var methodInfo = methodCallExpression.Method;

                        if (methodInfo.IsEnumerableAny())
                        {
                            if (methodCallExpression.Arguments[0] is MethodCallExpression previousExpression && previousExpression.Method.IsEnumerableIntersect())
                            {
                                if (previousExpression.Arguments[0] is MemberExpression sourceMember)
                                    return new TerminalGremlinExpression(parameter, sourceMember, previousExpression.Arguments[1].ToPWithin());

                                if (previousExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                    return new TerminalGremlinExpression(parameter, argument, previousExpression.Arguments[0].ToPWithin());
                            }
                            else
                                return new TerminalGremlinExpression(parameter, methodCallExpression.Arguments[0], P.Neq(new object[] { null }));
                        }
                        else if (methodInfo.IsEnumerableContains() || methodInfo.IsStepLabelContains())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression sourceMember && sourceMember.Expression == parameter)
                                return new TerminalGremlinExpression(parameter, sourceMember, P.Eq(methodCallExpression.Arguments[1].GetValue()));

                            if (methodCallExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                return new TerminalGremlinExpression(parameter, argument, methodCallExpression.Arguments[0].ToPWithin());
                        }
                        else if (methodInfo.IsStringStartsWith() || methodInfo.IsStringEndsWith() || methodInfo.IsStringContains())
                        {
                            if (methodInfo.IsStringStartsWith() && methodCallExpression.Arguments[0] is MemberExpression argumentExpression)
                            {
                                if (methodCallExpression.Object.GetValue() is string stringValue)
                                {
                                    return new TerminalGremlinExpression(
                                        parameter,
                                        argumentExpression,
                                        P.Within(Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => stringValue.Substring(0, i))
                                            .ToArray<object>()));
                                }
                            }
                            else if (methodCallExpression.Object is MemberExpression memberExpression)
                            {
                                if (methodCallExpression.Arguments[0].GetValue() is string str)
                                {
                                    return new TerminalGremlinExpression(
                                        parameter,
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
                throw new ExpressionNotSupportedException(expression, ex);
            }

            throw new ExpressionNotSupportedException(expression);
        }

        internal static P ToPWithin(this Expression expression)
        {
            if (expression.GetValue() is IEnumerable enumerable)
                return P.Within(enumerable.Cast<object>().ToArray());

            if (expression.GetValue() is StepLabel stepLabel)
                return P.Within(stepLabel);

            throw new ExpressionNotSupportedException(expression);
        }

        internal static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            if (expression.Body.StripConvert() is MemberExpression memberExpression)
                return memberExpression.Member;

            throw new ExpressionNotSupportedException(expression);
        }
    }
}
