using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;

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
                                        return new TerminalGremlinExpression(parameter, sourceMember, previousExpression.Arguments[1].ToPWithin());

                                    if (previousExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                        return new TerminalGremlinExpression(parameter, argument, previousExpression.Arguments[0].ToPWithin());
                                }
                                else
                                    return new TerminalGremlinExpression(parameter, methodCallExpression.Arguments[0], new P.Neq(null));
                            }
                            else if (methodInfo.IsEnumerableContains())
                            {
                                if (methodCallExpression.Arguments[0] is MemberExpression sourceMember && sourceMember.Expression == parameter)
                                    return new TerminalGremlinExpression(parameter, sourceMember, new P.Eq(methodCallExpression.Arguments[1].GetValue()));

                                if (methodCallExpression.Arguments[1] is MemberExpression argument && argument.Expression == parameter)
                                    return new TerminalGremlinExpression(parameter, argument, methodCallExpression.Arguments[0].ToPWithin());
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

        internal static P ToPWithin(this Expression expression)
        {
            if (expression.GetValue() is IEnumerable enumerable)
                return new P.Within(enumerable.Cast<object>().ToArray());

            throw new ExpressionNotSupportedException(expression);
        }

        public static PropertyInfo GetPropertyAccess(this LambdaExpression propertyAccessExpression)
        {
            Debug.Assert(propertyAccessExpression.Parameters.Count == 1);

            var parameterExpression = propertyAccessExpression.Parameters.Single();
            var propertyInfo = parameterExpression.MatchSimplePropertyAccess(propertyAccessExpression.Body);

            if (propertyInfo == null)
            {
                throw new ArgumentException(
                    "Invalid property access expression",
                    nameof(propertyAccessExpression));
            }

            var declaringType = propertyInfo.DeclaringType;
            var parameterType = parameterExpression.Type;

            if (declaringType != null
                && declaringType != parameterType
                && declaringType.GetTypeInfo().IsInterface
                && declaringType.GetTypeInfo().IsAssignableFrom(parameterType.GetTypeInfo()))
            {
                var propertyGetter = propertyInfo.GetMethod;
                var interfaceMapping = parameterType.GetTypeInfo().GetRuntimeInterfaceMap(declaringType);
                var index = Array.FindIndex(interfaceMapping.InterfaceMethods, p => propertyGetter.Equals(p));
                var targetMethod = interfaceMapping.TargetMethods[index];
                foreach (var runtimeProperty in parameterType.GetRuntimeProperties())
                {
                    if (targetMethod.Equals(runtimeProperty.GetMethod))
                    {
                        return runtimeProperty;
                    }
                }
            }

            return propertyInfo;
        }

        private static PropertyInfo MatchSimplePropertyAccess(
           this Expression parameterExpression, Expression propertyAccessExpression)
        {
            var propertyInfos = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

            return propertyInfos?.Count == 1 ? propertyInfos[0] : null;
        }

        private static IReadOnlyList<PropertyInfo> MatchPropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            var propertyInfos = new List<PropertyInfo>();

            MemberExpression memberExpression;

            do
            {
                memberExpression = RemoveTypeAs(propertyAccessExpression.RemoveConvert()) as MemberExpression;

                if (!(memberExpression?.Member is PropertyInfo propertyInfo))
                {
                    return null;
                }

                propertyInfos.Insert(0, propertyInfo);

                propertyAccessExpression = memberExpression.Expression;
            }
            while (RemoveTypeAs(memberExpression.Expression.RemoveConvert()) != parameterExpression);

            return propertyInfos;
        }

        private static Expression RemoveTypeAs(this Expression expression)
        {
            while ((expression?.NodeType == ExpressionType.TypeAs))
            {
                expression = ((UnaryExpression)expression.RemoveConvert()).Operand;
            }

            return expression;
        }

        private static Expression RemoveConvert(this Expression expression)
        {
            while (expression != null
                   && (expression.NodeType == ExpressionType.Convert
                       || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }
    }
}
