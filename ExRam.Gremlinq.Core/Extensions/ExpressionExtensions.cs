using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal abstract class ExpressionFragment
    {
        public static readonly ConstantExpressionFragment True = new ConstantExpressionFragment(true);
        public static readonly ConstantExpressionFragment Null = new ConstantExpressionFragment(default);

        protected ExpressionFragment(Expression? expression = default)
        {
            Expression = expression;
        }

        public Expression? Expression { get; }

        public static ExpressionFragment Create(Expression expression)
        {
            return expression.RefersToParameter()
                ? (ExpressionFragment)new ParameterExpressionFragment(expression)
                : expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression)
                    ? new StepLabelExpressionFragment(stepLabel, stepLabelExpression)
                    : new ConstantExpressionFragment(expression.GetValue());
        }
    }

    internal class ConstantExpressionFragment : ExpressionFragment
    {
        public ConstantExpressionFragment(object? value, Expression? expression = default) : base(expression)
        {
            if (value is IEnumerable enumerable && !(value is ICollection) && !(value is string))
                value = enumerable.Cast<object>().ToArray();

            Value = value;
        }

        public object? Value { get; }
    }

    internal class ParameterExpressionFragment : ExpressionFragment
    {
        public ParameterExpressionFragment(Expression expression) : base(expression)
        {
        }
    }

    internal class StepLabelExpressionFragment : ConstantExpressionFragment
    {
        public StepLabelExpressionFragment(StepLabel stepLabel, Expression? expression = default) : base(stepLabel, expression)
        {
        }
    }

    internal static class ExpressionSemanticsExtensions
    {
        private static readonly P P_Eq_True = P.Eq(true);
        private static readonly P P_Neq_Null = P.Neq(new object[] { null });

        public static ExpressionSemantics Flip(this ExpressionSemantics semantics)
        {
            return semantics switch
            {
                ExpressionSemantics.Contains => ExpressionSemantics.IsContainedIn,
                ExpressionSemantics.StartsWith => ExpressionSemantics.IsPrefixOf,
                ExpressionSemantics.EndsWith => ExpressionSemantics.IsSuffixOf,
                ExpressionSemantics.HasInfix => ExpressionSemantics.IsInfixOf,
                ExpressionSemantics.LowerThan => ExpressionSemantics.GreaterThan,
                ExpressionSemantics.GreaterThan => ExpressionSemantics.LowerThan,
                ExpressionSemantics.Equals => ExpressionSemantics.Equals,
                ExpressionSemantics.Intersects => ExpressionSemantics.Intersects,
                ExpressionSemantics.GreaterThanOrEqual => ExpressionSemantics.LowerThanOrEqual,
                ExpressionSemantics.LowerThanOrEqual => ExpressionSemantics.GreaterThanOrEqual,
                ExpressionSemantics.NotEquals => ExpressionSemantics.NotEquals,
                ExpressionSemantics.IsContainedIn => ExpressionSemantics.Contains,
                ExpressionSemantics.IsInfixOf => ExpressionSemantics.HasInfix,
                ExpressionSemantics.IsPrefixOf => ExpressionSemantics.StartsWith,
                ExpressionSemantics.IsSuffixOf => ExpressionSemantics.EndsWith,
                _ => throw new ArgumentOutOfRangeException(nameof(semantics), semantics, null)
            };
        }

        public static P ToP(this ExpressionSemantics semantics, object? value)
        {
            return semantics switch
            {
                ExpressionSemantics.Contains => P.Eq(value),
                ExpressionSemantics.IsPrefixOf when value is string stringValue => P.Within(Enumerable
                    .Range(0, stringValue.Length + 1)
                    .Select(i => stringValue.Substring(0, i))
                    .ToArray<object>()),
                ExpressionSemantics.HasInfix when value is string stringValue => stringValue.Length > 0
                    ? TextP.Containing(stringValue)
                    : P_Neq_Null,
                ExpressionSemantics.StartsWith when value is string stringValue => stringValue.Length > 0
                    ? TextP.StartingWith(stringValue)
                    : P_Neq_Null,
                ExpressionSemantics.EndsWith when value is string stringValue => stringValue.Length > 0
                    ? TextP.EndingWith(stringValue)
                    : P_Neq_Null,
                ExpressionSemantics.LowerThan => P.Lt(value),
                ExpressionSemantics.GreaterThan => P.Gt(value),
                ExpressionSemantics.Equals => P.Eq(value),
                ExpressionSemantics.NotEquals => P.Neq(value),
                ExpressionSemantics.Intersects => P.Within(value),
                ExpressionSemantics.GreaterThanOrEqual => P.Gte(value),
                ExpressionSemantics.LowerThanOrEqual => P.Lte(value),
                ExpressionSemantics.IsContainedIn => P.Within(value),
                ExpressionSemantics.IsInfixOf => throw new ExpressionNotSupportedException(),
                ExpressionSemantics.IsSuffixOf => throw new ExpressionNotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(semantics), semantics, null)
            };
        }
    }

    internal enum ExpressionSemantics
    {
        Equals,
        LowerThan,
        LowerThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Intersects,
        Contains,
        HasInfix,
        StartsWith,
        EndsWith,
        NotEquals,
        IsContainedIn,
        IsInfixOf,
        IsPrefixOf,
        IsSuffixOf,
    }

    internal enum MethodCallPattern
    {
        EnumerableIntersectAny,
        EnumerableAny,
        EnumerableContains,
        StringContains,
        StringStartsWith,
        StringEndsWith,
    }

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
                    case MemberExpression memberExpression when memberExpression.IsStepLabelValue():
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
                        var left = binaryExpression.Left.Strip();
                        var right = binaryExpression.Right.Strip();

                        if (binaryExpression.NodeType == ExpressionType.AndAlso || binaryExpression.NodeType == ExpressionType.OrElse)
                        {
                            /*if (left.TryToGremlinExpression() is { } leftExpression && right.TryToGremlinExpression() is { } rightExpression)
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
                            }*/
                        }
                        else
                        {
                            return new GremlinExpression(
                                ExpressionFragment.Create(left),
                                binaryExpression.NodeType.ToSemantics(),
                                ExpressionFragment.Create(right));
                        }

                        break;
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        var pattern = methodCallExpression.GetMethodCallSemantics();
                        var thisExpression = methodCallExpression.Arguments[0].Strip();

                        switch (pattern)
                        {
                            case MethodCallPattern.EnumerableIntersectAny:
                            {
                                var arguments = ((MethodCallExpression)thisExpression).Arguments;

                                return new GremlinExpression(
                                    ExpressionFragment.Create(arguments[0].Strip()),
                                    ExpressionSemantics.Intersects,
                                    ExpressionFragment.Create(arguments[1].Strip()));
                            }
                            case MethodCallPattern.EnumerableAny:
                            {
                                return new GremlinExpression(
                                    ExpressionFragment.Create(thisExpression),
                                    ExpressionSemantics.NotEquals,
                                    ExpressionFragment.Null);
                            }
                            case MethodCallPattern.EnumerableContains:
                            {
                                var argumentExpression = methodCallExpression.Arguments[1].Strip();

                                return new GremlinExpression(
                                    ExpressionFragment.Create(thisExpression),
                                    ExpressionSemantics.Contains,
                                    ExpressionFragment.Create(argumentExpression));
                            }
                            case MethodCallPattern.StringStartsWith:
                            case MethodCallPattern.StringEndsWith:
                            case MethodCallPattern.StringContains:
                            {
                                var instanceExpression = methodCallExpression.Object.Strip();
                                var argumentExpression = methodCallExpression.Arguments[0].Strip();

                                if (pattern == MethodCallPattern.StringStartsWith && argumentExpression is MemberExpression)
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
                                                MethodCallPattern.StringStartsWith => ExpressionSemantics.StartsWith,
                                                MethodCallPattern.StringContains => ExpressionSemantics.HasInfix,
                                                MethodCallPattern.StringEndsWith => ExpressionSemantics.EndsWith,
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

        public static MethodCallPattern GetMethodCallSemantics(this MethodCallExpression methodCallExpression)
        {
            var methodInfo = methodCallExpression.Method;

            if (methodInfo.IsStatic)
            {
                var thisExpression = methodCallExpression.Arguments[0].Strip();

                if (methodCallExpression.IsEnumerableAny())
                {
                    return thisExpression is MethodCallExpression previousMethodCallExpression && previousMethodCallExpression.IsEnumerableIntersect()
                        ? MethodCallPattern.EnumerableIntersectAny
                        : MethodCallPattern.EnumerableAny;
                }

                if (methodCallExpression.IsEnumerableContains())
                    return MethodCallPattern.EnumerableContains;
            }
            else if (methodCallExpression.IsStringStartsWith())
                return MethodCallPattern.StringStartsWith;
            else if (methodCallExpression.IsStringEndsWith())
                return MethodCallPattern.StringEndsWith;
            else if (methodCallExpression.IsStringContains())
                return MethodCallPattern.StringContains;

            throw new ExpressionNotSupportedException(methodCallExpression);
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
