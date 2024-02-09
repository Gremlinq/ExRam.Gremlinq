using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class WellKnownMethods
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        public static readonly MethodInfo ObjectToString = Get<object>(_ => _.ToString());
        public static readonly MethodInfo EnumerableAny = Get<IEnumerable<object>>(_ => _.Any());
        public static readonly MethodInfo EnumerableIntersect = Get<IEnumerable<object>>(_ => _.Intersect(default!));
        public static readonly MethodInfo EnumerableContainsElement = Get<IEnumerable<object>>(_ => _.Contains(default!));

        private static MethodInfo Get<TSource>(Expression<Action<TSource>> expression)
        {
            var method = ((MethodCallExpression)expression.Body).Method;

            return method.IsGenericMethod
                ? method.GetGenericMethodDefinition()
                : method;
        }
    }
}
