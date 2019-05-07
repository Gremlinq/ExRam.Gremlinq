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
        internal static IImmutableDictionary<MemberInfo, PropertyMetadata> ToCamelCase(this IImmutableDictionary<MemberInfo, PropertyMetadata> mapping)
        {
            return mapping
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kvp => new PropertyMetadata(
                        kvp.Value.IdentifierOverride.IfNone(kvp.Key.Name).ToCamelCase(),
                        kvp.Value.IgnoreDirective));
        }

        internal static IImmutableDictionary<MemberInfo, PropertyMetadata> ToLowerCase(this IImmutableDictionary<MemberInfo, PropertyMetadata> mapping)
        {
            return mapping
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kvp => new PropertyMetadata(
                        kvp.Value.IdentifierOverride.IfNone(kvp.Key.Name).ToLower(),
                        kvp.Value.IgnoreDirective));
        }
    }
}
