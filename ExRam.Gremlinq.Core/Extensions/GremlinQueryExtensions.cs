using System;
using System.Collections.Generic;
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

        internal static IGremlinQueryBase AddStep(this IGremlinQueryBase query, Step step)
        {
            return query.AsAdmin().ConfigureSteps(steps => steps.Push(step));
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

        /// <summary>
        /// Creates a continuation delegate from a <paramref name="sourceQuery"/> and <paramref name="targetQuery"/>
        /// when <paramref name="sourceQuery"/> is a prefix of <paramref name="targetQuery"/>.
        /// </summary>
        /// <typeparam name="TSourceQuery"></typeparam>
        /// <typeparam name="TTargetQuery"></typeparam>
        /// <param name="sourceQuery"></param>
        /// <param name="targetQuery"></param>
        /// <returns></returns>
        public static Func<TSourceQuery, TTargetQuery> CreateContinuationFrom<TSourceQuery, TTargetQuery>(this TSourceQuery sourceQuery, TTargetQuery targetQuery)
            where TSourceQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase
        {
            var sourceAdmin = sourceQuery.AsAdmin();
            var targetAdmin = targetQuery.AsAdmin();

            if (!ReferenceEquals(sourceAdmin.Environment, targetAdmin.Environment))
                throw new ArgumentException($"{nameof(sourceQuery)} and {nameof(targetQuery)} don't agree on environments.");

            using (var e1 = sourceAdmin.Steps.Reverse().GetEnumerator())
            {
                using (var e2 = targetAdmin.Steps.Reverse().GetEnumerator())
                {
                    var list = new List<Step>();

                    while (true)
                    {
                        var e1MoveNext = e1.MoveNext();

                        if (e2.MoveNext())
                        {
                            if (e1MoveNext)
                            {
                                if (!ReferenceEquals(e1.Current, e2.Current))
                                    throw new ArgumentException($"{nameof(sourceQuery)} is not a proper prefix of {nameof(targetQuery)}.");
                            }
                            else
                            {
                                do
                                {
                                    list.Add(e2.Current);
                                } while (e2.MoveNext());

                                break;
                            }
                        }
                        else
                        {
                            if (e1MoveNext)
                                throw new ArgumentException($"{nameof(sourceQuery)} must not have more steps than {nameof(targetQuery)}.");

                            break;
                        }
                    }

                    return _ => _
                        .AsAdmin()
                        .AddSteps(list)
                        .AsAdmin()
                        .ChangeQueryType<TTargetQuery>();
                }
            }
        }
     }
}
