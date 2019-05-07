using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Extensions;
using LanguageExt;

namespace System.Linq
{
    public static class ImmutableDictionaryExtensions
    {
        public static Option<TValue> TryGetValue<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dict, TKey key)
        {
            return ((IReadOnlyDictionary<TKey, TValue>)dict).TryGetValue(key);
        }

        internal static IImmutableDictionary<MemberInfo, MemberMetadata> ToCamelCase(this IImmutableDictionary<MemberInfo, MemberMetadata> mapping)
        {
            return mapping
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kpv => new MemberMetadata(
                        kpv.Value.Identifier.ToCamelCase(),
                        kpv.Value.IgnoreDirective));
        }

        internal static IImmutableDictionary<MemberInfo, MemberMetadata> ToLowerCase(this IImmutableDictionary<MemberInfo, MemberMetadata> mapping)
        {
            return mapping
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kpv => new MemberMetadata(
                        kpv.Value.Identifier.ToLower(),
                        kpv.Value.IgnoreDirective));
        }
    }
}
