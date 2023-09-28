namespace ExRam.Gremlinq.Core
{
    internal static class SpanExtensions
    {
        public static bool All<T>(this Span<T> span, Predicate<T> predicate) => ((ReadOnlySpan<T>)span).All(predicate);

        public static bool All<T, TState>(this Span<T> span, Func<T, TState, bool> predicate, TState state) => ((ReadOnlySpan<T>)span).All(predicate, state);

        public static bool All<T>(this ReadOnlySpan<T> span, Predicate<T> predicate) => span.All(static (value, predicate) => predicate(value), predicate);

        public static bool All<T, TState>(this ReadOnlySpan<T> span, Func<T, TState, bool> predicate, TState state)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (!predicate(span[i], state))
                    return false;
            }

            return true;
        }
    }
}
