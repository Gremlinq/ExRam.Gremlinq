using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public static class EnumerableExtensions
    {
        public static Traversal ToTraversal(this IEnumerable<Step> steps) => new(
            steps is Step[] array
                ? (Step[])array.Clone()
                : steps.ToArray(),
            Projection.Empty);

        internal static bool InternalAny(this IEnumerable enumerable)
        {
            if (enumerable is ICollection collection)
                return collection.Count > 0;

            var enumerator = enumerable.GetEnumerator();

            try
            {
                return enumerator.MoveNext();
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        internal static IAsyncEnumerable<TElement> ToNonNullAsyncEnumerable<TElement>(this IEnumerable enumerable)
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
