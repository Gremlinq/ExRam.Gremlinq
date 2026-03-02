using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        private static readonly LimitStep LimitGlobal2 = new(2, Scope.Global);

        /// <summary>
        /// Materializes the query results into an array.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement[]> ToArrayAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .ToAsyncEnumerable()
                .ToArrayAsync(ct);
        }

        /// <summary>
        /// Returns the first element of the query. Throws if the sequence is empty.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement> FirstAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                    .Push(LimitStep.LimitGlobal1))
                .ToAsyncEnumerable()
                .FirstAsync(ct);
        }

        /// <summary>
        /// Returns the first element of the query, or a default value if the sequence is empty.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement?> FirstOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                    .Push(LimitStep.LimitGlobal1))
                .ToAsyncEnumerable()
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Returns the only element of the query. Throws if the sequence does not contain exactly one element.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement> SingleAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                    .Push(LimitGlobal2))
                .ToAsyncEnumerable()
                .SingleAsync(ct);
        }

        /// <summary>
        /// Returns the only element of the query, or a default value if the sequence is empty. Throws if the sequence contains more than one element.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement?> SingleOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                    .Push(LimitGlobal2))
                .ToAsyncEnumerable()
                .SingleOrDefaultAsync(ct);
        }

        /// <summary>
        /// Returns the last element of the query. Throws if the sequence is empty.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement> LastAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                    .Push(TailStep.TailGlobal1))
                .ToAsyncEnumerable()
                .FirstAsync(ct);
        }

        /// <summary>
        /// Returns the last element of the query, or a default value if the sequence is empty.
        /// </summary>
        /// <typeparam name="TElement">The element type of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="ct">A cancellation token.</param>
        public static ValueTask<TElement?> LastOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ConfigureSteps<IGremlinQuery<TElement>>(static traversal => traversal
                    .Push(TailStep.TailGlobal1))
                .ToAsyncEnumerable()
                .FirstOrDefaultAsync(ct);
        }

        internal static Traversal ToTraversal(this IGremlinQueryBase query) => query
            .AsAdmin().Steps;
    }
}
