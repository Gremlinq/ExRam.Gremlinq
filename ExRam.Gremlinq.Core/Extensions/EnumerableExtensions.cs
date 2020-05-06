using System;
using System.Collections;

namespace ExRam.Gremlinq.Core
{
    internal static class EnumerableExtensions
    {
        public static bool InternalAny(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            return enumerator.MoveNext();
        }
    }
}
