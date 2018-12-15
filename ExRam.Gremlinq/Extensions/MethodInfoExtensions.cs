using System.Linq;
using System.Linq.Expressions;

namespace System.Reflection
{
    internal static class MethodInfoExtensions
    {
        private static readonly MethodInfo EnumerableAny = Get(() => Enumerable.Any<object>(default))?.GetGenericMethodDefinition();
        private static readonly MethodInfo EnumerableIntersect = Get(() => Enumerable.Intersect<object>(default, default))?.GetGenericMethodDefinition();
        private static readonly MethodInfo EnumerableContains = Get(() => Enumerable.Contains<object>(default, default))?.GetGenericMethodDefinition();
        private static readonly MethodInfo StringStartsWith = Get(() => string.Empty.StartsWith(default));

        public static bool IsEnumerableAny(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == EnumerableAny;
        }

        public static bool IsEnumerableContains(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == EnumerableContains;
        }

        public static bool IsEnumerableIntersect(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == EnumerableIntersect;
        }

        public static bool IsStringStartsWith(this MethodInfo methodInfo)
        {
            return methodInfo == StringStartsWith;
        }

        private static MethodInfo Get(Expression<Action> expression)
        {
            return (expression.Body as MethodCallExpression)?.Method;
        }
    }
}
