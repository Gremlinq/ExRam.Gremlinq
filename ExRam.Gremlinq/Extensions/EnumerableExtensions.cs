using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static bool Intersects<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            throw new NotImplementedException("This method is to be used within expression and therefore has no implementation.");
        }
    }
}