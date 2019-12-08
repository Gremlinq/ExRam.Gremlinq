using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class MethodInfoExtensions
    {
        // ReSharper disable ReturnValueOfPureMethodIsNotUsed
        private static readonly MethodInfo EnumerableAny = Get(() => Enumerable.Any<object>(default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableIntersect = Get(() => Enumerable.Intersect<object>(default, default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableIntersectsStepLabel = Get(() => EnumerableExtensions.Intersect<object>(default, default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableContainsElement = Get(() => Enumerable.Contains<object>(default, default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo EnumerableContainsStepLabel = Get(() => EnumerableExtensions.Contains<object>(default, default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo StepLabelContainsElement = Get(() => StepLabelExtensions.Contains<object>(default, default))?.GetGenericMethodDefinition()!;
        private static readonly MethodInfo StringStartsWith = Get(() => string.Empty.StartsWith(default(string)));
        private static readonly MethodInfo StringContains = Get(() => string.Empty.Contains(default(string)));
        private static readonly MethodInfo StringEndsWith = Get(() => string.Empty.EndsWith(default(string)));
        // ReSharper restore ReturnValueOfPureMethodIsNotUsed

        public static bool IsEnumerableAny(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == EnumerableAny;
        }

        public static bool IsEnumerableContains(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && (methodInfo.GetGenericMethodDefinition() == EnumerableContainsElement || methodInfo.GetGenericMethodDefinition() == EnumerableContainsStepLabel);
        }

        public static bool IsEnumerableIntersect(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && (methodInfo.GetGenericMethodDefinition() == EnumerableIntersect || methodInfo.GetGenericMethodDefinition() == EnumerableIntersectsStepLabel);
        }

        public static bool IsStringStartsWith(this MethodInfo methodInfo)
        {
            return methodInfo == StringStartsWith;
        }

        public static bool IsStringContains(this MethodInfo methodInfo)
        {
            return methodInfo == StringContains;
        }

        public static bool IsStringEndsWith(this MethodInfo methodInfo)
        {
            return methodInfo == StringEndsWith;
        }

        public static bool IsStepLabelContains(this MethodInfo methodInfo)
        {
            return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == StepLabelContainsElement;
        }

        private static MethodInfo Get(Expression<Action> expression)
        {
            return (expression.Body as MethodCallExpression)?.Method;
        }
    }
}
