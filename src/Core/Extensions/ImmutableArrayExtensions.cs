#if (!NET7_0_OR_GREATER)
namespace System.Collections.Immutable
{
    internal static class ImmutableArrayExtensions
    {
        public static ImmutableArray<T> ToImmutableArray<T>(this Span<T> span) => ((ReadOnlySpan<T>)span).ToImmutableArray();

        public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlySpan<T> span)
        {
            var builder = ImmutableArray
                .CreateBuilder<T>(span.Length);

            for (var i = 0; i < span.Length; i++)
            {
                builder
                    .Add(span[i]);
            }

            return builder.ToImmutable();
        }
    }
}
#endif
