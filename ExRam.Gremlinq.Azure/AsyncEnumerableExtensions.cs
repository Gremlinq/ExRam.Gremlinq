using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Documents.Client
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> enumerable, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
            return AsyncEnumerable.CreateEnumerable(
                () =>
                {
                    var current = default(TResult);
                    var e = enumerable.GetEnumerator();

                    return AsyncEnumerable.CreateEnumerator(
                        async ct =>
                        {
                            if (await e.MoveNext(ct).ConfigureAwait(false))
                            {
                                current = await selector(e.Current, ct).ConfigureAwait(false);
                                return true;
                            }

                            return false;
                        },
                        () => current,
                        e.Dispose);
                });
        }
    }
}