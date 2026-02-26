// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion
namespace ExRam.Gremlinq.Core
{
    internal static class SanitizedTypeArrayCache<TElement>
    {
        public static Type[]? Sanitize(Type[] types)
        {
            if (types.Any(static type => type.IsAssignableFrom(typeof(TElement)) || type == typeof(object)))
                return null;

            var sanitizedTypes = types
                .Where(static edgeType => typeof(TElement).IsAssignableFrom(edgeType))
                .ToArray();

            if (sanitizedTypes.Length == 0)
                throw new InvalidOperationException($"The graph model does not contain any types assignable to any of {string.Join(',', types.Select(type => type.FullName))} in the type hierarchy of {typeof(TElement).FullName}.");

            return sanitizedTypes;
        }
    }

    internal static class SanitizedTypeArrayCache<TElement, T1>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15)]);
    }

    internal static class SanitizedTypeArrayCache<TElement, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        public static readonly Type[]? Types = SanitizedTypeArrayCache<TElement>.Sanitize([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16)]);
    }
}
