#if !NET10_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class AsyncEnumerable
    {
        private const string NoElements = "Source sequence doesn't contain any elements.";
        private const string MoreThanOneElement = "Source sequence contains more than one element.";

        public static IAsyncEnumerable<T> Empty<T>()
        {
            return Core();

            static async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken _ = default)
            {
                yield break;
            }
        }

        public static async ValueTask<TSource?> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
            await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
            {
                return item;
            }

            return default;
        }

        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
            await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
            {
                return item;
            }

            throw new InvalidOperationException(NoElements);
        }

        public static async ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await using (var e = source.WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
            {
                if (!await e.MoveNextAsync())
                    throw new InvalidOperationException(NoElements);

                var result = e.Current;

                if (await e.MoveNextAsync())
                    throw new InvalidOperationException(MoreThanOneElement);

                return result;
            }
        }

        public static async ValueTask<TSource?> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await using (var e = source.WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
            {
                if (!await e.MoveNextAsync())
                    return default;

                var result = e.Current;

                if (await e.MoveNextAsync())
                    throw new InvalidOperationException(MoreThanOneElement);

                return result;
            }
        }

        public static async ValueTask<TSource[]> ToArrayAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var list = new List<TSource>();

            await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
            {
                list.Add(item);
            }

            return list.ToArray();
        }
    }
}
#endif
