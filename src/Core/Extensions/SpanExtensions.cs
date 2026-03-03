using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExRam.Gremlinq.Core
{
    internal static class SpanExtensions
    {
        public readonly ref struct SpanCast<TSource>
            where TSource : class
        {
            private readonly ReadOnlySpan<TSource> _source;

            public SpanCast(ReadOnlySpan<TSource> source)
            {
                _source = source;
            }

            public ReadOnlySpan<TTarget> To<TTarget>()
                where TTarget : class => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TSource, TTarget>(ref MemoryMarshal.GetReference(_source)), _source.Length);
        }

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

        public static SpanCast<TSource> Cast<TSource>(this Span<TSource> span) where TSource : class => new(span);

        public static SpanCast<TSource> Cast<TSource>(this ReadOnlySpan<TSource> span) where TSource : class => new (span);
    }
}
