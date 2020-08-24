using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExtensions
    {
        public static ValueTask<TElement[]> ToArrayAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            return query.ToAsyncEnumerable().ToArrayAsync(ct);
        }

        public static ValueTask<TElement> FirstAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            return query.Cast<TElement>().Limit(1).ToAsyncEnumerable().FirstAsync(ct);
        }

        public static ValueTask<TElement> FirstOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            return query.Cast<TElement>().Limit(1).ToAsyncEnumerable().FirstOrDefaultAsync(ct);
        }

        public static ValueTask<TElement> SingleAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            return query.Cast<TElement>().Limit(1).ToAsyncEnumerable().SingleAsync(ct);
        }

        public static ValueTask<TElement> SingleOrDefaultAsync<TElement>(this IGremlinQueryBase<TElement> query, CancellationToken ct = default)
        {
            return query.Cast<TElement>().Limit(1).ToAsyncEnumerable().SingleOrDefaultAsync(ct);
        }

        internal static bool IsNone(this IGremlinQueryBase query)
        {
            return query.AsAdmin().Steps.PeekOrDefault() is NoneStep;
        }

        internal static bool IsIdentity(this IGremlinQueryBase query)
        {
            return query.AsAdmin().Steps.IsEmpty;
        }

        internal static Traversal ToTraversal(this IGremlinQueryBase query)
        {
            return query.AsAdmin().ToTraversal();
        }        
     }
}
