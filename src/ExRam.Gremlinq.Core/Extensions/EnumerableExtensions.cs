using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    internal static class EnumerableExtensions
    {
        public static bool InternalAny(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            return enumerator.MoveNext();
        }

        public static IAsyncEnumerable<TElement> ToNonNullAsyncEnumerable<TElement>(this IEnumerable enumerable)
        {
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TElement> Core(CancellationToken ct)
            {
                foreach (TElement element in enumerable)
                {
                    ct.ThrowIfCancellationRequested();

                    if (element is not null)
                        yield return element;
                }
            }
        }
    }
}
