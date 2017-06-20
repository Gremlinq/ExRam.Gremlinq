using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    internal static class ImmutableCollectionsExtensions
    {
        public static IImmutableDictionary<TKey, IImmutableList<TValue>> Add<TKey, TValue>(this IImmutableDictionary<TKey, IImmutableList<TValue>> dictionary, TKey key, TValue value)
        {
            return dictionary.SetItem(
                key,
                dictionary
                    .TryGetValue(key)
                    .Match(
                        list => list.Add(value),
                        () => ImmutableList.Create(value)));
        }
    }
}