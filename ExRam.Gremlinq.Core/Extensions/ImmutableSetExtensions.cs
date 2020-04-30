using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal static class ImmutableSetExtensions
    {
        public static IImmutableSet<TElement> AddRange<TElement>(this IImmutableSet<TElement> hashSet, IEnumerable<TElement> elements)
        {
            foreach (var element in elements)
            {
                hashSet = hashSet.Add(element);
            }

            return hashSet;
        }
    }
}
