using System.Linq.Async;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        public static ValueTask<TElement[]> ToArrayAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .ToAsyncEnumerable()
            .ToArrayAsync(ct);

        public static ValueTask<TElement> FirstAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .Cast<TElement>()
            .Limit(1)
            .ToAsyncEnumerable()
            .FirstAsync(ct);

        public static ValueTask<TElement?> FirstOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .Cast<TElement>()
            .Limit(1)
            .ToAsyncEnumerable()
            .FirstOrDefaultAsync(ct);

        public static ValueTask<TElement> SingleAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .Cast<TElement>()
            .Limit(2)
            .ToAsyncEnumerable()
            .SingleAsync(ct);

        public static ValueTask<TElement?> SingleOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .Cast<TElement>()
            .Limit(2)
            .ToAsyncEnumerable()
            .SingleOrDefaultAsync(ct);

        internal static Traversal ToTraversal(this IGremlinQueryBase query) => query
            .AsAdmin().Steps;
    }
}
