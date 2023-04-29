namespace ExRam.Gremlinq.Core
{
    internal static class SpanExtensions
    {
        public static bool All<T>(this Span<T> span, Predicate<T> predicate) => ((ReadOnlySpan<T>)span).All(predicate);

        public static bool All<T>(this ReadOnlySpan<T> span, Predicate<T> predicate)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (!predicate(span[i]))
                    return false;
            }

            return true;
        }
    }
}
