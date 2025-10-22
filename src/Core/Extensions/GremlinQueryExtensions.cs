using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        private static readonly LimitStep LimitGlobal2 = new(2, Scope.Global);

        public static ValueTask<TElement[]> ToArrayAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .ToAsyncEnumerable()
            .ToArrayAsync(ct);

        public static ValueTask<TElement> FirstAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                .Push(LimitStep.LimitGlobal1))
            .ToAsyncEnumerable()
            .FirstAsync(ct);

        public static ValueTask<TElement?> FirstOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                .Push(LimitStep.LimitGlobal1))
            .ToAsyncEnumerable()
            .FirstOrDefaultAsync(ct);

        public static ValueTask<TElement> SingleAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                .Push(LimitGlobal2))
            .ToAsyncEnumerable()
            .SingleAsync(ct);

        public static ValueTask<TElement?> SingleOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                .Push(LimitGlobal2))
            .ToAsyncEnumerable()
            .SingleOrDefaultAsync(ct);

        public static ValueTask<TElement> LastAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                .Push(TailStep.TailGlobal1))
            .ToAsyncEnumerable()
            .FirstAsync(ct);

        public static ValueTask<TElement?> LastOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default) => query
            .AsAdmin()
            .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                .Push(TailStep.TailGlobal1))
            .ToAsyncEnumerable()
            .FirstOrDefaultAsync(ct);

        internal static Traversal ToTraversal(this IGremlinQueryBase query) => query
            .AsAdmin().Steps;
    }
}
