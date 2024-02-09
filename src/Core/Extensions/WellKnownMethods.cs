using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class WellKnownMethods
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        public static readonly MethodInfo ObjectToString = Get(_ => _.ToString());
        public static readonly MethodInfo EnumerableAny = Get(_ => Enumerable.Any<object>(default!));
        public static readonly MethodInfo EnumerableIntersect = Get(_ => Enumerable.Intersect<object>(default!, default!));
        public static readonly MethodInfo EnumerableContainsElement = Get(_ => Enumerable.Contains<object>(default!, default!));

        private static MethodInfo Get(Expression<Action<object>> expression)
        {
            var method = ((MethodCallExpression)expression.Body).Method;

            return method.IsGenericMethod
                ? method.GetGenericMethodDefinition()
                : method;
        }
    }
}
