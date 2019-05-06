using System.Collections.Generic;
using System.Collections.Immutable;
using LanguageExt;

namespace System.Linq
{
    public static class ImmutableDictionaryExtensions
    {
        public static Option<TValue> TryGetValue<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dict, TKey key)
        {
            return ((IReadOnlyDictionary<TKey, TValue>)dict).TryGetValue(key);
        }
    }
}