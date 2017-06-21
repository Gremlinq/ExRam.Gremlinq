using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    internal static class ImmutableCollectionsExtensions
    {
        public static IImmutableDictionary<TKey, IImmutableSet<TValue>> Add<TKey, TValue>(this IImmutableDictionary<TKey, IImmutableSet<TValue>> dictionary, TKey key, TValue value)
        {
            return dictionary.SetItem(
                key,
                dictionary
                    .TryGetValue(key)
                    .Match(
                        list => list.Add(value),
                        () => ImmutableHashSet.Create(value)));
        }
    }
}