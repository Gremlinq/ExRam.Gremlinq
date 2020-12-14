using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class ImmutableDictionaryExtensions
    {
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
        
        public static IImmutableDictionary<MemberInfo, MemberMetadata> UseCamelCaseNames(this IImmutableDictionary<MemberInfo, MemberMetadata> names)
        {
            return names.ConfigureNames((member, key) => key.RawKey is string name
                ? name.ToCamelCase()
                : key);
        }

        public static IImmutableDictionary<MemberInfo, MemberMetadata> UseLowerCaseNames(this IImmutableDictionary<MemberInfo, MemberMetadata> names)
        {
            return names.ConfigureNames((member, key) => key.RawKey is string name
                ? name.ToLower()
                : key);
        }
    }
}
