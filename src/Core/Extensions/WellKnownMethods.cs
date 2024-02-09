using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class WellKnownMethods
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        public static readonly MethodInfo ObjectToString = Get<object>(static _ => _.ToString());
        public static readonly MethodInfo EnumerableAny = Get(static () => Enumerable.Any<object>(default!)).GetGenericMethodDefinition();
        public static readonly MethodInfo EnumerableIntersect = Get(static () => Enumerable.Intersect<object>(default!, default!)).GetGenericMethodDefinition();
#pragma warning disable 8625
        public static readonly MethodInfo EnumerableContainsElement = Get(static () => Enumerable.Contains<object>(default!, default)).GetGenericMethodDefinition();
#pragma warning restore 8625

        private static MethodInfo Get(Expression<Action> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }

        private static MethodInfo Get<TSource>(Expression<Action<TSource>> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}
