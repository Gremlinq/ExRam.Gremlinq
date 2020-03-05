using System.Collections;

namespace ExRam.Gremlinq.Core
{
    public static class EnumerableExtensions
    {
        internal static bool InternalAny(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            return enumerator.MoveNext();
        }
    }
}
