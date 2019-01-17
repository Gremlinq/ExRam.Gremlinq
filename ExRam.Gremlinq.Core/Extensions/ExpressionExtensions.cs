using System.Reflection;
using ExRam.Gremlinq.Core;

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

        public static GremlinExpression ToGremlinExpression<TSource, TResult>(this Expression<Func<TSource, TResult>> expression)
        {
            if (expression.Parameters.Count != 1)
                throw new ExpressionNotSupportedException(expression);

            return expression.Body.ToGremlinExpression(expression.Parameters[0]);
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
                            return new TerminalGremlinExpression(parameter, memberExpression, new P.Eq(true));

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
                                    return new TerminalGremlinExpression(parameter, sourceMember, P.Within.From(previousExpression.Arguments[1]));

                                if (previousExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                    return new TerminalGremlinExpression(parameter, argument, P.Within.From(previousExpression.Arguments[0]));
                            }
                            else
                                return new TerminalGremlinExpression(parameter, methodCallExpression.Arguments[0], new P.Neq(null));
                        }
                        else if (methodInfo.IsEnumerableContains())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression sourceMember && sourceMember.Expression == parameter)
                                return new TerminalGremlinExpression(parameter, sourceMember, new P.Eq(methodCallExpression.Arguments[1].GetValue()));

                            if (methodCallExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                return new TerminalGremlinExpression(parameter, argument, P.Within.From(methodCallExpression.Arguments[0]));
                        }
                        else if (methodInfo.IsStringStartsWith())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression argumentExpression)
                            {
                                if (methodCallExpression.Object.GetValue() is string stringValue)
                                {
                                    return new TerminalGremlinExpression(
                                        parameter,
                                        argumentExpression,
                                        new P.Within(Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => stringValue.Substring(0, i))
                                            .ToArray<object>()));
                                }
                            }
                            else if (methodCallExpression.Object is MemberExpression memberExpression)
                            {
                                if (methodCallExpression.Arguments[0].GetValue() is string lowerBound)
                                {
                                    string upperBound;

                                    if (lowerBound.Length == 0)
                                        return new TerminalGremlinExpression(parameter, memberExpression, P.True);

                                    if (lowerBound[lowerBound.Length - 1] == char.MaxValue)
                                        upperBound = lowerBound + char.MinValue;
                                    else
                                    {
                                        var upperBoundChars = lowerBound.ToCharArray();

                                        upperBoundChars[upperBoundChars.Length - 1]++;
                                        upperBound = new string(upperBoundChars);
                                    }

                                    return new TerminalGremlinExpression(parameter, memberExpression, new P.Between(lowerBound, upperBound));
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
    }
}
