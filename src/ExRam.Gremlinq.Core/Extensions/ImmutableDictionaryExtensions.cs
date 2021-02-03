using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class ImmutableDictionaryExtensions
    {
        private static readonly ConcurrentDictionary<object, object> FastDictionaries = new();

        internal static IImmutableDictionary<MemberInfo, MemberMetadata> ConfigureNames(this IImmutableDictionary<MemberInfo, MemberMetadata> metadata, Func<MemberInfo, Key, Key> transformation)
        {
            return metadata
                .SetItems(metadata
                    .Select(kvp => new KeyValuePair<MemberInfo, MemberMetadata>(
                        kvp.Key,
                        new MemberMetadata(
                            transformation(kvp.Key, kvp.Value.Key),
                            kvp.Value.SerializationBehaviour))));
        }

        internal static IReadOnlyDictionary<TKey, TValue> Fast<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dict)
            where TKey : notnull
        {
            return (IReadOnlyDictionary<TKey, TValue>)FastDictionaries
                .GetOrAdd(
                    dict,
                    closureDict => ((IImmutableDictionary<TKey, TValue>)closureDict).ToDictionary(x => x.Key, x => x.Value));
        }
        
        public static IImmutableDictionary<MemberInfo, MemberMetadata> UseCamelCaseNames(this IImmutableDictionary<MemberInfo, MemberMetadata> names)
        {
            return names.ConfigureNames((_, key) => key.RawKey is string name
                ? name.ToCamelCase()
                : key);
        }

        public static IImmutableDictionary<MemberInfo, MemberMetadata> UseLowerCaseNames(this IImmutableDictionary<MemberInfo, MemberMetadata> names)
        {
            return names.ConfigureNames((_, key) => key.RawKey is string name
                ? name.ToLower()
                : key);
        }
    }
}
