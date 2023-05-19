namespace System.Linq.Async
{
    internal static class AsyncEnumerable
    {
        private const string NoElements = "Source sequence doesn't contain any elements.";
        private const string MoreThanOneElement = "Source sequence contains more than one element.";

        private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly Func<CancellationToken, IAsyncEnumerator<T>> _enumeratorFactory;

            public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> enumeratorFactory) => _enumeratorFactory = enumeratorFactory;

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct)
            {
                ct.ThrowIfCancellationRequested();

                return _enumeratorFactory(ct);
            }
        }

        public static IAsyncEnumerable<T> Empty<T>()
        {
            return Create(Core);

            static async IAsyncEnumerator<T> Core(CancellationToken ct)
            {
                yield break;
            }
        }

        public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> enumeratorFactory)
        {
            return new AnonymousAsyncEnumerable<T>(enumeratorFactory);
        }

        public static async ValueTask<TSource?> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
            await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
            {
                return item;
            }

            return default;
        }

        public static async ValueTask<TSource?> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken ct = default)
        {
            var hasLast = false;
            var last = default(TSource)!;

            await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
            {
                last = item;
                hasLast = true;
            }

            return hasLast
                ? last
                : default;
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
            await using (var e = source.WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
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
            await using (var e = source.WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
            {
                if (!await e.MoveNextAsync())
                    throw new InvalidOperationException(NoElements);

                var result = e.Current;

                if (await e.MoveNextAsync())
                    return default;

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
