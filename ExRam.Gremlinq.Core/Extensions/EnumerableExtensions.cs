using System;
using System.Collections;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public static class EnumerableExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> source, StepLabel<TSource> stepLabel)
        {
            throw new InvalidOperationException($"{nameof(EnumerableExtensions)}.{nameof(Contains)} is not intended to be executed. It's use is only valid within expressions.");
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> source, StepLabel<TSource[]> stepLabel)
        {
            throw new InvalidOperationException($"{nameof(EnumerableExtensions)}.{nameof(Intersect)} is not intended to be executed. It's use is only valid within expressions.");
        }

        internal static bool InternalAny(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            return enumerator.MoveNext();
        }

        internal static IEnumerable<Step> HandleAnonymousQueries(this IEnumerable<Step> steps)
        {
            using (var e = steps.GetEnumerator())
            {
                if (!e.MoveNext())
                    yield return IdentityStep.Instance;
                else
                {
                    do
                    {
                        yield return e.Current;
                    } while (e.MoveNext());
                }
            }
        }
    }
}
